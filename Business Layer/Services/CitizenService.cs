namespace Business_Layer.Services;

public class CitizenService(
    UserManager<User> userManager,
    SignInManager<User> signInManager,
    IMapper mapper,
    AppDbContext context,
    TokenService tokenService) : ICitizenService
{
    public async Task<Response<GetUserDto>> ClientRegister(CreateUserDto dto)
    {
        var response = new Response<GetUserDto>();

        try
        {
            // var existingUser = await userManager.FindByEmailAsync(dto.Email.Trim());
            //
            // var existingUserId = existingUser?.Id;
            //
            // var isNameExists = await context.Users.AnyAsync
            //     (u => u.Id != existingUserId && u.UserName == dto.UserName.Trim());
            //
            // if (isNameExists)
            // {
            //     response.Error = "Name already taken";
            //
            //     return response;
            // }

            var isNameExists =
                await context.Users.AnyAsync(u => u.UserName == dto.FirstName.Trim() + dto.LastName.Trim());

            if (isNameExists)
            {
                response.Error = "Firstname and Lastname already exist, Please Login";

                return response;
            }

            var isEmailExists = await context.Users.AnyAsync(u => u.Email == dto.Email.Trim());

            if (isEmailExists)
            {
                response.Error = "Email Already Exists, Please Login";

                return response;
            }

            var newUser = mapper.Map<User>(dto);

            newUser.UserType = UserType.Citizen;
            newUser.UserName = dto.FirstName.Trim() + dto.LastName.Trim();

            var result = await userManager.CreateAsync(newUser, dto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));

                response.Error = $"Password validation failed: {errors}";

                return response;
            }

            var otpResponse = await tokenService.RequestOTP(newUser);

            if (!otpResponse.Success)
            {
                response.Error = response.Error;

                return response;
            }

            var otp = otpResponse.Result;

            var newClient = new Citizen
            {
                UserId = newUser.Id,
                User = null!,
                OTP = otp,
                OTPExpirationTime = DateTime.UtcNow.AddMinutes(2)
            };

            context.Clients.Add(newClient);

            await context.SaveChangesAsync();

            await userManager.AddToRoleAsync(newUser, Roles.Citizen);

            response.Success = true;

            response.Result = mapper.Map<GetUserDto>(newUser);
        }

        catch (Exception e)
        {
            response.Error = e.Message;
        }

        return response;
    }

    public async Task<Response<AuthResponse>> Login(LoginDto dto)
    {
        var response = new Response<AuthResponse>();

        // var isValidEmail = new EmailAddressAttribute().IsValid(dto.EmailPhone);
        // User? user;
        //
        // if (isValidEmail)
        // {
        //     user = await userManager.FindByEmailAsync(dto.EmailPhone);
        // }
        //
        // else
        // {
        //     user = await userManager.phone(dto.EmailPhone);
        // }

        var emailPhone = dto.EmailPhone.Trim();

        var user = await context.Users
            .Where(u => u.Email == emailPhone || u.PhoneNumber == emailPhone)
            .FirstOrDefaultAsync();

        if (user == null)
        {
            response.Error = "Unable to Log In";

            return response;
        }

        var result = await signInManager.PasswordSignInAsync(user, dto.Password, false, false);

        if (!result.Succeeded)
        {
            response.Error = "Unable to Log In";

            return response;
        }

        var client = await context.Clients.FirstAsync(c => c.UserId == user.Id);

        if (!user.EmailConfirmed)
        {
            if (client.OTPExpirationTime == null || DateTime.UtcNow >= client.OTPExpirationTime)
            {
                var otpResponse = await tokenService.RequestOTP(user);

                if (!otpResponse.Success)
                {
                    response.Error = response.Error;

                    return response;
                }

                var otp = otpResponse.Result;

                client.OTP = otp;

                client.OTPExpirationTime = DateTime.UtcNow.AddMinutes(2);

                await context.SaveChangesAsync();
            }

            response.Result = new AuthResponse
            {
                IsEmailConfirmed = false
            };

            response.Success = true;

            return response;
        }

        if (dto.Fcm != null)
        {
            user.Fcm = dto.Fcm;

            await context.SaveChangesAsync();
        }

        response.Success = true;

        response.Result = new AuthResponse
        {
            Token = await tokenService.CreateJwtToken(user),
            UserId = user.Id,
            User = mapper.Map<GetUserDto>(user),
            Type = user.UserType.ToString(),
            IsEmailConfirmed = true
        };

        return response;
    }
}