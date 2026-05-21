using sabidos.Domain.Entities;

namespace sabidos.Domain.Interfaces
{
    public interface IAgendaRepository
    {
        Task<AgendaEvent?> GetByIdAsync(string id);
        Task<IEnumerable<AgendaEvent>> GetByUserIdAsync(string userId);
        Task AddAsync(AgendaEvent agendaEvent);
        Task UpdateAsync(AgendaEvent agendaEvent);
        Task DeleteAsync(string id);
    }
}
