namespace Business_Layer.Services;

public class DepartmentService(IMapper mapper, AppDbContext context) : IDepartmentService
{
    public async Task<Response<List<GetDepartmentDto>>> GetAll()
    {
        var response = new Response<List<GetDepartmentDto>>();

        var buffets = await context.Departments.ToListAsync();

        response.Success = true;
        
        response.Result = mapper.Map<List<GetDepartmentDto>>(buffets);

        return response;
    }
}