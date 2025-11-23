namespace Business_Layer.IServices;

public interface ICitizenService
{
    Task<Response<GetUserDto>> Register(CreateCitizenDto dto);
    
    Task<Response<GetUserDto>> Update(UpdateUserDto dto);

    Task<Response<AuthResponse>> Login(User dto);
}