namespace Business_Layer.IServices;

public interface IComplaintService
{
    Task<Response<GetComplaintDto>> Add(AddComplaintDto dto);

    Task<Response<GetComplaintDto>> GetDetails(int id);
}