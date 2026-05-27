using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sabidos.Application.Services;
using System.Security.Claims;

namespace sabidos.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class MissionsController : ControllerBase
{
    private readonly DailyMissionService _missionService;

    public MissionsController(DailyMissionService missionService)
    {
        _missionService = missionService;
    }

    [HttpGet("daily")]
    public async Task<IActionResult> GetDailyMissions()
    {
        var userId = User.FindFirst("user_id")?.Value ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var response = await _missionService.GetDailyMissions(userId);
        return Ok(response);
    }

    [HttpPost("reroll/{missionId}")]
    public async Task<IActionResult> RerollMission(string missionId)
    {
        var userId = User.FindFirst("user_id")?.Value ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        try
        {
            var response = await _missionService.RerollMission(userId, missionId);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("claim/{missionId}")]
    public async Task<IActionResult> ClaimReward(string missionId)
    {
        var userId = User.FindFirst("user_id")?.Value ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        try
        {
            var response = await _missionService.ClaimMissionReward(userId, missionId);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
