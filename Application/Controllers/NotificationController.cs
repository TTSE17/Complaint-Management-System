using System.Security.Claims;

namespace Application.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NotificationController(INotificationService notificationService) : ControllerBase
{
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Get()
    {
        var userId = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var response = await notificationService.GetAllNotifications(Convert.ToInt32(userId));

        return Ok(response);
    }
}