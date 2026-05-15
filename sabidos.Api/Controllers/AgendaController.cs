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
public class AgendaController : ControllerBase
{
    private readonly AgendaService _agendaService;

    public AgendaController(AgendaService agendaService)
    {
        _agendaService = agendaService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value 
                         ?? User.FindFirst("user_id")?.Value;

            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var events = await _agendaService.GetUserEvents(userId);
            
            var response = events.Select(e => new AgendaEventResponse
            {
                Id = e.Id,
                Title = e.Title,
                Date = e.Date,
                CreatedAt = e.CreatedAt
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
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value 
                         ?? User.FindFirst("user_id")?.Value;

            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var ev = await _agendaService.GetUserEvents(userId);
            var result = ev.FirstOrDefault(e => e.Id == id);

            if (result == null) return NotFound();

            return Ok(new AgendaEventResponse
            {
                Id = result.Id,
                Title = result.Title,
                Date = result.Date,
                CreatedAt = result.CreatedAt
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AgendaEventCreateRequest request)
    {
        try
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value 
                         ?? User.FindFirst("user_id")?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "Usuário não identificado no Token." });

            var agendaEvent = new AgendaEvent
            {
                Title = request.Title,
                Date = request.Date,
                UserId = userId
            };

            await _agendaService.CreateEvent(agendaEvent);

            var response = new AgendaEventResponse
            {
                Id = agendaEvent.Id,
                Title = agendaEvent.Title,
                Date = agendaEvent.Date,
                CreatedAt = agendaEvent.CreatedAt
            };

            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message, detail = ex.InnerException?.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] AgendaEventCreateRequest request)
    {
        try
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value 
                         ?? User.FindFirst("user_id")?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var events = await _agendaService.GetUserEvents(userId);
            var existingEvent = events.FirstOrDefault(e => e.Id == id);

            if (existingEvent == null)
                return NotFound();

            existingEvent.Title = request.Title;
            existingEvent.Date = request.Date;

            await _agendaService.UpdateEvent(existingEvent);

            return NoContent();
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
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value 
                         ?? User.FindFirst("user_id")?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            await _agendaService.DeleteEvent(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
