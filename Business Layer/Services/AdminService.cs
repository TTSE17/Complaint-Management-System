namespace Business_Layer.Services;

public class AdminService(IMapper mapper, TokenService tokenService) : IAdminService
{
    public async Task<Response<AuthResponse>> Login(User user)
    {
        var response = new Response<AuthResponse>();

        var token = await tokenService.CreateJwtToken(user);

        var userDto = mapper.Map<GetUserDto>(user);

        response.Result = new AuthResponse
        {
            Token = token,
            UserId = user.Id,
            User = userDto,
            Type = user.UserType.ToString(),
            IsEmailConfirmed = true
        };

        response.Success = true;

        return response;
    }
}