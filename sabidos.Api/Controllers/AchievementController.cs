using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sabidos.Application.Interfaces;
using sabidos.Application.Services;

namespace sabidos.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AchievementController : Controller
    {
        private readonly PointService _pointService;
        private readonly IAchievementRepository _repo;
        private readonly AchievementService _achievement;
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirst("user_id")?.Value;

            if (userId == null)
                return Unauthorized();

            var stats = await _repo.GetUserStats(userId);

            return Ok(stats);
        }
    }
}
