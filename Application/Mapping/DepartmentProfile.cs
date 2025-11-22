namespace Application.Mapping;

public class DepartmentProfile : Profile
{
    public DepartmentProfile()
    {
        CreateMap<Department, GetDepartmentDto>();
    }
}