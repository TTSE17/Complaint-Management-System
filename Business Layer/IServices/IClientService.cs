namespace Business_Layer.IServices;

public interface IClientService
{
    Task<Response<GetUserDto>> ClientRegister(CreateUserDto dto);

    Task<Response<AuthResponse>> Login(LoginDto dto);
}