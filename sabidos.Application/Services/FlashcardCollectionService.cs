using sabidos.Domain.Entities;
using sabidos.Domain.Interfaces;

namespace sabidos.Application.Services;

public class FlashcardCollectionService
{
    private readonly IFlashcardCollectionRepository _repository;
    private readonly IFlashcardRepository _flashcardRepository;

    public FlashcardCollectionService(IFlashcardCollectionRepository repository, IFlashcardRepository flashcardRepository)
    {
        _repository = repository;
        _flashcardRepository = flashcardRepository;
    }

    public async Task CreateNewCollection(FlashcardCollection collection)
    {
        collection.CreatedAt = DateTime.UtcNow;
        await _repository.AddAsync(collection);
    }

    public async Task<IEnumerable<FlashcardCollection>> GetCollectionsByUserId(string userId)
    {
        return await _repository.GetByUserIdAsync(userId);
    }

    public async Task DeleteCollection(string id)
    {
        // Exclui todos os flashcards pertencentes à coleção primeiro
        await _flashcardRepository.DeleteByCollectionIdAsync(id);
        
        // Em seguida, exclui a coleção
        await _repository.DeleteAsync(id);
    }
}
