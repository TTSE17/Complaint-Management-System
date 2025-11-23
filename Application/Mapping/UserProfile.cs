namespace Application.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<CreateUserDto, User>();
            
            CreateMap<UpdateUserDto, User>();

            CreateMap<User, GetUserDto>();
        }
    }
}