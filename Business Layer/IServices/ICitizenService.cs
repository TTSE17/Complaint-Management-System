namespace Business_Layer.IServices;

public interface ICitizenService
{
    Task<Response<GetUserDto>> ClientRegister(CreateCitizenDto dto);

    Task<Response<AuthResponse>> Login(User dto);
}