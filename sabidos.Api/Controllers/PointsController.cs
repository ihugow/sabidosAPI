
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sabidos.Application.DTOs;
using sabidos.Application.Services;
using sabidos.Domain.Entities;
using sabidos.Domain.Interfaces;
using System.Security.Claims;
using System.Text.Json;

namespace sabidos.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PointsController : ControllerBase
{
    private readonly PointService _pointService;
    private readonly IPointRepository _repo;
    private readonly AchievementService _achievement;

    public PointsController(
        PointService pointService,
        IPointRepository repo,
        AchievementService achievement)
    {
        _pointService = pointService;
        _repo = repo;
        _achievement = achievement;
    }
    [HttpPost("earn")]
    public async Task<IActionResult> Earn([FromBody] EarnRequest request)
    {
        var userId = User.FindFirst("user_id")?.Value;

        if (userId == null)
            return Unauthorized();

        object parsedData = null;

        switch (request.Action)
        {
            case PointActionType.FlashcardRespondido:
                parsedData = request.Data.Deserialize<FlashcardData>();
                break;

            case PointActionType.PomodoroCompleto:
                parsedData = request.Data.Deserialize<PomodoroData>();
                break;

            case PointActionType.ResumoCriado:
                parsedData = null;
                break;
        }

        int points = _pointService.CalculatePoints(
            request.Action,
            parsedData
        );

        await _repo.AddTransaction(
            userId,
            points,
            request.Action.ToString()
        );

        await _repo.UpdatePoints(userId, points);

        int total = await _repo.GetUserPoints(userId);

        var unlockedBefore = await _achievement.GetUserAchievementsIds(userId);
        await _achievement.CheckAchievements(userId);
        var unlockedAfter = await _achievement.GetUserAchievementsIds(userId);

        var newlyUnlocked = unlockedAfter.Except(unlockedBefore).ToList();

        return Ok(new
        {
            earnedPoints = points,
            totalPoints = total,
            unlockedAchievements = newlyUnlocked
        });
    }
    [HttpGet("me")]
    public async Task<IActionResult> GetMyPoints()
    {
        var userId = User.FindFirst("user_id")?.Value;
        if (userId == null)
            return Unauthorized();
        int points = await _repo.GetUserPoints(userId);
        //await _achievement.CheckAchievements(userId, total);
        return Ok(new
        {
            totalPoints = points
        });
    }

}