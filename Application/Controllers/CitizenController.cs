namespace Application.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CitizenController(ICitizenService citizenService) : ControllerBase
{
    [HttpPost("Register")]
    public async Task<IActionResult> Register(CreateUserDto request)
    {
        var response = await citizenService.ClientRegister(request);

        if (response.Success)
            return Ok(response);

        return BadRequest(response);
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login(LoginDto request)
    {
        var response = await citizenService.Login(request);

        if (!response.Success)
            return Unauthorized(response);

        var isEmailConfirmed = response.Result.IsEmailConfirmed;

        if (!isEmailConfirmed)
            return Conflict();

        return Ok(response);
    }
}