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
    public async Task<Response<GetDepartmentDto>> GetById(int id)
    {
        var response = new Response<GetDepartmentDto>();

        var exists = await context.Departments.AnyAsync(c => c.Id == id);

        if (!exists)
        {
            response.Error = $"Department {id} does not exist";
            return response;
        }

        var department = await context.Departments
            .Where(c => c.Id == id)
            .Select(c => new GetDepartmentDto
            {
                Id = c.Id,
                Name = c.Name
            })
            .FirstAsync();

        response.Result = department;
        response.Success = true;

        return response;
    }

}