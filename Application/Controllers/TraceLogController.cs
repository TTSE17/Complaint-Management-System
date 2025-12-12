using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TraceLogController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TraceLogController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var logs = await _context.TraceLogs
                .OrderByDescending(x => x.Timestamp)
                .ToListAsync();

            return Ok(logs);
        }
    }

}
