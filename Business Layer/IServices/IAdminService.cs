namespace Business_Layer.IServices;

public interface IAdminService
{
    Task<Response<AuthResponse>> Login(User user);
}