using Business_Layer.Consts;
using Microsoft.AspNetCore.Authorization;

namespace Application.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ComplaintController(IComplaintService complaintService) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = Roles.Citizen)]
    public async Task<IActionResult> Create(AddComplaintDto dto)
    {
        var response = await complaintService.Add(dto);

        if (response.Success)
            return Ok(response);

        return BadRequest(response);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetItemDetails(int id)
    {
        var response = await complaintService.GetDetails(id);

        if (response.Success)
        {
            return Ok(response);
        }

        return NotFound(response);
    }
}