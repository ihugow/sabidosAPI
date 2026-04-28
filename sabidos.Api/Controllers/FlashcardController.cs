using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sabidos.Application.DTOs;
using sabidos.Application.Services;
using sabidos.Domain.Entities;
using System.Security.Claims;

namespace sabidos.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class FlashcardController : ControllerBase
{
    private readonly FlashcardService _flashcardService;

    public FlashcardController(FlashcardService flashcardService)
    {
        _flashcardService = flashcardService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] FlashcardCreateRequest request)
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var flashcard = new Flashcard
            {
                Front = request.Front,
                Back = request.Back,
                UserId = userId,
            };

            await _flashcardService.CreateNewFlashcard(flashcard);

            var response = new FlashcardResponse
            {
                Id = flashcard.Id,
                Front = flashcard.Front,
                Back = flashcard.Back,
                UserId = flashcard.UserId,
                CreatedAt = flashcard.CreatedAt
            };

            // Retornamos o status 201 (Created)
            return CreatedAtAction(nameof(Create), new { id = response.Id }, response);
        }
        catch (Exception ex)
        {
            // Em produção, você logaria o erro. Aqui retornamos um erro genérico.
            return BadRequest(new { message = ex.Message });
        }
    }
}