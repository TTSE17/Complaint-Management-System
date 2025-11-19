using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using OtpNet;

namespace Business_Layer.Services
{
    public class TokenService(
        JwtOptions options,
        UserManager<User> userManager,
        AppDbContext context,
        IEmailService emailService)
    {
        public async Task<string> CreateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.UserName!),
                new(ClaimTypes.Email, user.Email!)
            };

            var roles = await userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = options.Issuer,
                Audience = options.Audience,


                Subject = new ClaimsIdentity
                (
                    claims
                ),

                Expires = DateTime.UtcNow.AddMinutes(options.LifeTime),

                SigningCredentials = new SigningCredentials
                (
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.SigningKey)),
                    SecurityAlgorithms.HmacSha256
                ),
            };
            SecurityToken securityToken = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(securityToken);
        }

        public string GenerateOTP()
        {
            // 1️⃣ Secret key (shared between client and server)
            // Typically a Base32-encoded string (e.g., from a QR code)
            const string base32Secret = "JBSWY3DPEHPK3PXP"; // Example secret

            // 2️⃣ Decode the Base32 secret to bytes
            var secretBytes = Base32Encoding.ToBytes(base32Secret);

            // 3️⃣ Create a TOTP generator (default: 30-second step, 6 digits)
            var totp = new Totp(secretBytes, 119);

            // 4️⃣ Generate the current OTP
            var otp = totp.ComputeTotp();

            Console.WriteLine($"Your current TOTP code is: {otp}");

            // Optional: Get remaining time before code expires
            var remaining = totp.RemainingSeconds();
            Console.WriteLine($"Seconds remaining: {remaining}");

            return otp;
        }

        private int GenerateRandomCode()
        {
            var otp = new Random().Next(100000, 999999);

            return otp;
        }

        public async Task<Response<bool>> CheckOtpCode(string email, string otp)
        {
            var response = new Response<bool>();

            var client = await context.Clients.Include(c => c.User)
                .FirstOrDefaultAsync(c => c.User.Email!.ToLower() == email.ToLower());

            if (client == null)
            {
                response.Error = "Email Not Found!";

                return response;
            }

            if (client.OTP != otp)
            {
                response.Error = "OTP is Wrong!";

                return response;
            }

            if (client.OTPExpirationTime <= DateTime.UtcNow)
            {
                response.Error = "OTP Expired, Please Request OTP Again!";

                return response;
            }

            client.User.EmailConfirmed = true;

            await context.SaveChangesAsync();

            response.Success = true;

            response.Result = true;

            return response;
        }

        public async Task<Response<string>> RequestOTP(string email)
        {
            var response = new Response<string>();

            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                response.Error = "Email Not Found!";

                return response;
            }

            var otpResponse = await RequestOTP(user);

            if (!otpResponse.Success)
            {
                response.Error = otpResponse.Error;

                return response;
            }

            var otp = otpResponse.Result;

            var client = await context.Clients.FirstAsync(c => c.UserId == user.Id);

            client.OTP = otp;

            client.OTPExpirationTime = DateTime.UtcNow.AddMinutes(2);

            await context.SaveChangesAsync();
            
            response.Success = true;

            response.Result = "OTP Send Success!";

            return response;
        }

        public async Task<Response<string>> RequestOTP(User user)
        {
            var response = new Response<string>();

            var otp = GenerateRandomCode().ToString();

            var body = $@"
                    <h1>Hello {user.FirstName},</h1>
                    <p>Thank you for registering with us! Please confirm your email address</p>
                    <h3> Your otp code is: {otp}
                    <p>If you did not create an account, please ignore this email.</p>
                    <p>Thank you!</p>";

            var emailResponse = await emailService.SendEmail(user.Email!, "Confirm Your Email Address", body);

            if (!emailResponse.Success)
            {
                response.Error = emailResponse.Error;

                return response;
            }

            response.Result = otp;

            response.Success = true;

            return response;
        }
    }
}