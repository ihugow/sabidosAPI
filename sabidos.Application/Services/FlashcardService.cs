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
        // Aqui você poderia validar se o usuário atingiu o limite diário,
        // ou adicionar lógica de experiência (XP)
        flashcard.CreatedAt = DateTime.UtcNow;
        await _repository.AddAsync(flashcard);
    }
}