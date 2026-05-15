using sabidos.Domain.Entities;
using sabidos.Domain.Interfaces;

namespace sabidos.Application.Services;

public class AgendaService
{
    private readonly IAgendaRepository _repository;

    public AgendaService(IAgendaRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<AgendaEvent>> GetUserEvents(string userId)
    {
        return await _repository.GetByUserIdAsync(userId);
    }

    public async Task CreateEvent(AgendaEvent agendaEvent)
    {
        await _repository.AddAsync(agendaEvent);
    }

    public async Task UpdateEvent(AgendaEvent agendaEvent)
    {
        await _repository.UpdateAsync(agendaEvent);
    }

    public async Task DeleteEvent(string id)
    {
        await _repository.DeleteAsync(id);
    }
}
