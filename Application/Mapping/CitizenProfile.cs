namespace Application.Mapping;

public class CitizenProfile : Profile
{
    public CitizenProfile()
    {
        CreateMap<Citizen, GetUserDto>();
    }
}