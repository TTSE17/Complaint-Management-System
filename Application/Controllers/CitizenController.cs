using Business_Layer.Consts;
using Microsoft.AspNetCore.Identity;

namespace Application.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CitizenController(IUserService userService) : ControllerBase
{
    [HttpPost("Register")]
    public async Task<IActionResult> Register(CreateUserDto request)
    {
        if (request.Password.Trim() == "")
        {
            var res = new Response<string>
            {
                Error = "Password is required"
            };

            return BadRequest(res);
        }

        var response = await userService.Register(request, UserType.Citizen, Roles.Citizen);

        if (response.Success)
            return Ok(response);

        return BadRequest(response);
    }
}