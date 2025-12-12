namespace Business_Layer.IServices;

public interface IDepartmentService
{
    Task<Response<List<GetDepartmentDto>>> GetAll();

    Task<Response<GetDepartmentDto>> GetById(int id);

}