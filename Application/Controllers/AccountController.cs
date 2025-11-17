namespace Application.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController(IUserService userService) : ControllerBase
{
    [HttpPost("Login")]
    public async Task<IActionResult> Login(LoginDto request)
    {
        var response = await userService.Login(request);

        if (response.Success)
            return Ok(response);

        return Unauthorized(response);
    }
}