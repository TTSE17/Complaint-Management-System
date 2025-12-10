namespace Business_Layer.IServices;

public interface IAdminService
{
    Task<Response<AuthResponse>> Login(User user);


    Task<Response<int>> GetUsersCount();

    Task<Response<int>> GetComplaintsCount();


    Task<Response<int>> GetComplaintsRejectedCount();


    Task<Response<int>> GetComplaintsResolvedCount();

    Task<Response<int>> GetComplaintsPendingCount();


}