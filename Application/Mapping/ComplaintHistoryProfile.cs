namespace Application.Mapping;

public class ComplaintHistoryProfile : Profile
{
    public ComplaintHistoryProfile()
    {
        CreateMap<ComplaintHistory, GetComplaintHistoryDto>();
    }
}