using Business_Layer.Services;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers;
[Authorize]
[Route("api/[controller]")]
[ApiController]
public class AdminController(IAdminService adminService) : ControllerBase
{
    [HttpGet("users-count")]
    public async Task<IActionResult> GetUsersCount()
    {
        var result = await adminService.GetUsersCount();
        return Ok(result);
    }

    [HttpGet("complaints-count")]
    public async Task<IActionResult> GetComplaintsCount()
    {
        var result = await adminService.GetComplaintsCount();
        return Ok(result);
    }

    [HttpGet("complaints-rejected")]
    public async Task<IActionResult> GetComplaintsRejected()
    {
        var result = await adminService.GetComplaintsRejectedCount();
        return Ok(result);
    }

    [HttpGet("complaints-resolved")]
    public async Task<IActionResult> GetComplaintsResolved()
    {
        var result = await adminService.GetComplaintsResolvedCount();
        return Ok(result);
    }

    [HttpGet("complaints-pending")]
    public async Task<IActionResult> GetComplaintsPending()
    {
        var result = await adminService.GetComplaintsPendingCount();
        return Ok(result);
    }

    [HttpGet("weekly-completed")]
    public async Task<IActionResult> GetWeeklyCompleted()
    {
        var result = await adminService.GetWeeklyCompletedStats();
        return Ok(result);
    }


}
