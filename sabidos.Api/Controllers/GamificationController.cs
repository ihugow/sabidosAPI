using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sabidos.Application.Services;

namespace sabidos.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/gamification")]
public class GamificationController
    : ControllerBase
{
    private readonly GamificationService
        _service;

    public GamificationController(
        GamificationService service)
    {
        _service = service;
    }

    [HttpGet("profile")]
    public async Task<IActionResult>
        GetProfile()
    {
        var userId =
            User.FindFirst("user_id")?.Value;

        if (userId == null)
            return Unauthorized();

        var profile =
            await _service.GetProfile(userId);

        return Ok(profile);
    }
}