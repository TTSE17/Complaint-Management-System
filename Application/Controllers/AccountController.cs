namespace Application.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController(IAccountService accountService) : ControllerBase
{
    [HttpPost("Login")]
    public async Task<IActionResult> Login(LoginDto request)
    {
        var response = await accountService.Login(request);

        if (!response.Success)
            return Unauthorized(response);

        var isEmailConfirmed = response.Result.IsEmailConfirmed;

        if (!isEmailConfirmed)
            return Conflict();

        return Ok(response);
    }
}