
using Supabase;
using sabidos.Domain.Entities;
using sabidos.Domain.Interfaces;

namespace sabidos.Infrastructure.Repositories;

public class SupabaseFlashcardRepository : IFlashcardRepository
{
    private readonly Client _supabaseClient;

    // O cliente do Supabase será injetado aqui via Dependency Injection
    public SupabaseFlashcardRepository(Client supabaseClient)
    {
        _supabaseClient = supabaseClient;
    }

    public async Task AddAsync(Flashcard flashcard)
    {
        // No Supabase, enviamos o objeto diretamente para a tabela
        await _supabaseClient.From<Flashcard>().Insert(flashcard);
    }

    public async Task<IEnumerable<Flashcard>> GetByUserIdAsync(string userId)
    {
        var result = await _supabaseClient
            .From<Flashcard>()
            .Where(x => x.UserId == userId)
            .Get();

        return result.Models;
    }

    public async Task<Flashcard?> GetByIdAsync(Guid id)
    {
        var result = await _supabaseClient
            .From<Flashcard>()
            .Where(x => x.Id == id)
            .Single();

        return result;
    }

    // Implementaremos Update e Delete conforme avançarmos
    public Task UpdateAsync(Flashcard flashcard) => throw new NotImplementedException();
    public Task DeleteAsync(Guid id) => throw new NotImplementedException();
}