namespace Business_Layer.IServices
{
    public interface IUserService
    {
        Task<Response<GetUserDto>> Register(CreateUserDto dto, UserType userType, string role);

        Task<Response<AuthResponse>> Login(LoginDto dto);

        // Task<Response<GetUserDto>> Update(UpdateUserDto dto);

        Task Logout();
    }
}