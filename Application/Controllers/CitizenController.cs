namespace Application.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CitizenController(ICitizenService citizenService) : ControllerBase
{
    [HttpPost("Register")]
    public async Task<IActionResult> Register(CreateCitizenDto request)
    {
        var response = await citizenService.Register(request);

        if (response.Success)
            return Ok(response);

        return BadRequest(response);
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateUserDto request)
    {
        var response = await citizenService.Update(request);

        if (response.Success)
            return Ok(response);

        return BadRequest(response);
    }
}