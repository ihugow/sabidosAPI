using sabidos.Domain.Entities;

namespace sabidos.Domain.Interfaces
{
    public interface IFlashcardCollectionRepository
    {
        Task<FlashcardCollection?> GetByIdAsync(string id);
        Task<IEnumerable<FlashcardCollection>> GetByUserIdAsync(string userId);
        Task AddAsync(FlashcardCollection collection);
        Task UpdateAsync(FlashcardCollection collection);
        Task DeleteAsync(string id);
    }
}
