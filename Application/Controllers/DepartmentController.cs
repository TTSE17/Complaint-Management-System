namespace Application.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DepartmentController(IDepartmentService departmentService) : ControllerBase
{
    [HttpGet]
    // [Authorize]
    public async Task<IActionResult> GetAll()
    {
        var response = await departmentService.GetAll();

        return Ok(response);
    }
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var response = await departmentService.GetById(id);

        return Ok(response);
    }

}