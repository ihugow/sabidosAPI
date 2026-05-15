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
public class FlashcardCollectionController : ControllerBase
{
    private readonly FlashcardCollectionService _collectionService;

    public FlashcardCollectionController(FlashcardCollectionService collectionService)
    {
        _collectionService = collectionService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] FlashcardCollectionCreateRequest request)
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var collection = new FlashcardCollection
            {
                Name = request.Name,
                Color = request.Color,
                UserId = userId,
            };

            await _collectionService.CreateNewCollection(collection);

            var response = new FlashcardCollectionResponse
            {
                Id = collection.Id,
                Name = collection.Name,
                Color = collection.Color,
                UserId = collection.UserId,
                CreatedAt = collection.CreatedAt
            };

            return CreatedAtAction(nameof(Create), new { id = response.Id }, response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> GetMyCollections()
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();
                
            var collections = await _collectionService.GetCollectionsByUserId(userId);
            
            var response = collections.Select(c => new FlashcardCollectionResponse
            {
                Id = c.Id,
                Name = c.Name,
                Color = c.Color,
                UserId = c.UserId,
                CreatedAt = c.CreatedAt
            });
            
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var collections = await _collectionService.GetCollectionsByUserId(userId);
            var collection = collections.FirstOrDefault(c => c.Id == id);

            if (collection == null) return NotFound();

            var response = new FlashcardCollectionResponse
            {
                Id = collection.Id,
                Name = collection.Name,
                Color = collection.Color,
                UserId = collection.UserId,
                CreatedAt = collection.CreatedAt
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            await _collectionService.DeleteCollection(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
