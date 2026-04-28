using sabidos.Domain.Entities;

namespace sabidos.Domain.Interfaces
{
    public interface IFlashcardRepository
    {
        Task<Flashcard?> GetByIdAsync(Guid id);
        Task<IEnumerable<Flashcard>> GetByUserIdAsync(string userId);
        Task AddAsync(Flashcard flashcard);
        Task UpdateAsync(Flashcard flashcard);
        Task DeleteAsync(Guid id);
    }
}
