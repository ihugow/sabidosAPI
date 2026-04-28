using sabidos.Domain.Entities;
using sabidos.Domain.Interfaces;

namespace sabidos.Application.Services;

public class FlashcardService
{
    private readonly IFlashcardRepository _repository;

    public FlashcardService(IFlashcardRepository repository)
    {
        _repository = repository;
    }

    public async Task CreateNewFlashcard(Flashcard flashcard)
    {
        flashcard.CreatedAt = DateTime.UtcNow;
        await _repository.AddAsync(flashcard);
    }
    
    public async Task<IEnumerable<Flashcard>> GetFlashcardsByCollectionId(string collectionId)
    {
        return await _repository.GetByCollectionIdAsync(collectionId);
    }

    public async Task DeleteFlashcard(string id)
    {
        await _repository.DeleteAsync(id);
    }
}
