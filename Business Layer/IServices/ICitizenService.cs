namespace Business_Layer.IServices;

public interface ICitizenService
{
    Task<Response<GetUserDto>> ClientRegister(CreateUserDto dto);

    Task<Response<AuthResponse>> Login(LoginDto dto);
}