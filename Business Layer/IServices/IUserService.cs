namespace Business_Layer.IServices
{
    public interface IUserService
    {
        Task<Response<User>> Register(CreateUserDto dto);

        Task<Response<GetUserDto>> Update(UpdateUserDto dto);
    }
}