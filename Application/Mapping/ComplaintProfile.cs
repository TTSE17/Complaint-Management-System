namespace Application.Mapping;

public class ComplaintProfile : Profile
{
    public ComplaintProfile()
    {
        CreateMap<Complaint, GetComplaintDto>()
            .ForMember(des => des.DepartmentName, opt =>
                opt.MapFrom(src => src.Department.Name))
            .ForMember(des => des.Attachments, opt =>
                opt.MapFrom(src => src.Attachments.Select(attachment => attachment.FilePath).ToList()))
            .ForMember(des => des.Citizen, opt =>
                opt.MapFrom(src => src.Citizen.User))
            ;

        CreateMap<AddComplaintDto, Complaint>();
    }
}