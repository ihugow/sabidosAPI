using sabidos.Domain.Entities;

namespace sabidos.Domain.Interfaces
{
    public interface IFlashcardRepository
    {
        Task<Flashcard?> GetByIdAsync(string id);
        Task<IEnumerable<Flashcard>> GetByUserIdAsync(string userId);
        Task<IEnumerable<Flashcard>> GetByCollectionIdAsync(string collectionId);
        Task AddAsync(Flashcard flashcard);
        Task UpdateAsync(Flashcard flashcard);
        Task DeleteAsync(string id);
        Task DeleteByCollectionIdAsync(string collectionId);
    }
}

