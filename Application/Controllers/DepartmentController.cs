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
}