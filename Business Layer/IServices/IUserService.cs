namespace Business_Layer.IServices
{
    public interface IUserService
    {
        Task<Response<AuthResponse>> Login(LoginDto dto);

        // Task<Response<GetUserDto>> Update(UpdateUserDto dto);

        Task Logout();
    }
}