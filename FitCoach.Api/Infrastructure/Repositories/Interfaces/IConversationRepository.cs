using FitCoach.Api.Domain.Entities;

namespace FitCoach.Api.Infrastructure.Repositories.Interfaces;

// Contract for conversation data access.
// Services depend on this interface — never on the concrete implementation.
public interface IConversationRepository
{
    Task<Conversation?> GetByIdAsync(string id);
    Task<List<Conversation>> GetByUserIdAsync(string userId);
    Task<Conversation> CreateAsync(Conversation conversation);
    Task UpdateAsync(Conversation conversation);
}