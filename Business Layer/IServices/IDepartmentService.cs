namespace Business_Layer.IServices;

public interface IDepartmentService
{
    Task<Response<List<GetDepartmentDto>>> GetAll();
}