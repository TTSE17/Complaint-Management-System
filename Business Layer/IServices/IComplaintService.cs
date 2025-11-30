using System.Linq.Expressions;

namespace Business_Layer.IServices;

public interface IComplaintService
{
    Task<Response<List<GetComplaintDto>>> GetAll(Expression<Func<Complaint, bool>>? criteria = null);

    Task<Response<GetComplaintDto>> Add(AddComplaintDto dto);

    Task<Response<GetComplaintDto>> GetDetails(int id);

    Task<Response<GetComplaintDto>> Update(UpdateComplaintDto dto);

    Task<Response<bool>> Delete(int id);

    Task<Response<List<GetComplaintSnapshot>>> GetComplaintHistory(int complaintId);
}