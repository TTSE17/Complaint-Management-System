namespace Application.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CitizenController(ICitizenService citizenService) : ControllerBase
{
    [HttpPost("Register")]
    public async Task<IActionResult> Register(CreateCitizenDto request)
    {
        var response = await citizenService.ClientRegister(request);

        if (response.Success)
            return Ok(response);

        return BadRequest(response);
    }
}