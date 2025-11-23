namespace Business_Layer.Services;

public class CitizenService(
    IUserService userService,
    UserManager<User> userManager,
    IMapper mapper,
    AppDbContext context,
    TokenService tokenService) : ICitizenService
{
    public async Task<Response<GetUserDto>> ClientRegister(CreateCitizenDto dto)
    {
        var response = new Response<GetUserDto>();

        await using var transaction = await context.Database.BeginTransactionAsync();

        try
        {
            dto.UserType = UserType.Citizen;

            var userResult = await userService.Register(dto);

            if (!userResult.Success)
            {
                response.Error = userResult.Error;

                return response;
            }

            var user = userResult.Result;

            var otpResponse = await tokenService.RequestOTP(user);

            if (!otpResponse.Success)
            {
                response.Error = otpResponse.Error;

                return response;
            }

            var otp = otpResponse.Result;

            var newClient = new Citizen
            {
                UserId = user.Id,
                User = null!,
                OTP = otp,
                OTPExpirationTime = DateTime.UtcNow.AddMinutes(2)
            };

            context.Citizens.Add(newClient);

            await context.SaveChangesAsync();

            await transaction.CommitAsync();

            await userManager.AddToRoleAsync(user, Roles.Citizen);

            response.Result = mapper.Map<GetUserDto>(user);

            response.Success = true;
        }

        catch (Exception e)
        {
            await transaction.RollbackAsync();

            response.Error = e.Message;
        }

        return response;
    }

    public async Task<Response<AuthResponse>> Login(User user)
    {
        var response = new Response<AuthResponse>();

        var client = await context.Citizens.FirstAsync(c => c.UserId == user.Id);

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

        var userDto = mapper.Map<GetUserDto>(user);

        response.Result = new AuthResponse
        {
            Token = await tokenService.CreateJwtToken(user),
            UserId = user.Id,
            User = userDto,
            Type = user.UserType.ToString(),
            IsEmailConfirmed = true
        };

        response.Success = true;

        return response;
    }
}