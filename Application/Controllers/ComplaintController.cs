namespace Application.Controllers;

[Route("api/[controller]")]
[ApiController]
// [Authorize]
public class ComplaintController(IComplaintService complaintService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllComplaints(int? citizenId = null, string? status = null)
    {
        var complaintStatus = ComplaintStatus.Pending;

        if (!string.IsNullOrEmpty(status))
        {
            if (!Enum.TryParse(status, ignoreCase: true, out complaintStatus))
            {
                return BadRequest("Invalid status. Valid values are: Pending, Resolved, and Rejected.");
            }
        }

        var response = await complaintService.GetAll(c =>
            (string.IsNullOrEmpty(status) || c.Status == complaintStatus) &&
            (citizenId == null || c.CitizenId == citizenId)
        );

        return Ok(response);
    }

    [HttpPost]
    // [Authorize(Roles = Roles.Citizen)]
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

    [HttpPut]
    // [Authorize(Roles = Roles.Citizen)]
    public async Task<IActionResult> Update(UpdateComplaintDto dto)
    {
        var response = await complaintService.Update(dto);

        if (response.Success)
            return Ok(response);

        return BadRequest(response);
    }

    [HttpDelete("Delete")]
    // [Authorize(Roles = Roles.Customer)]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await complaintService.Delete(id);

        if (response.Success)
            return Ok(response);

        return BadRequest(response);
    }

    [HttpGet("History/{id:int}")]
    public async Task<IActionResult> GetComplaintHistory(int id)
    {
        var response = await complaintService.GetComplaintHistory(id);

        if (response.Success)
        {
            return Ok(response);
        }

        return NotFound(response);
    }
}