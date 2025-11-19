namespace Application.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TokenController(TokenService tokenService) : ControllerBase
{
    [HttpPost("Check-OTP")]
    public async Task<IActionResult> CheckOTP(string email, string otp)
    {
        if (otp.Length != 6)
            return BadRequest(new { Error = "The code must contain exactly 6 digits" });

        var response = await tokenService.CheckOtpCode(email.Trim(), otp.Trim());

        return response.Success ? Ok(response) : BadRequest(response);
    }

    [HttpPost("Send-OTP")]
    public async Task<IActionResult> SendOTP(string email)
    {
        var response = await tokenService.RequestOTP(email.Trim());

        return response.Success ? Ok(response) : BadRequest(response);
    }
}