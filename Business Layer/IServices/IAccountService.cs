namespace Business_Layer.IServices;

public interface IAccountService
{
    Task<Response<AuthResponse>> Login(LoginDto dto);


    Task Logout();
}