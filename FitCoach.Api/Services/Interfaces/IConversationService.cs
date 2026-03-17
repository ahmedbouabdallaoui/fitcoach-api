using FitCoach.Api.Domain.Entities;

namespace FitCoach.Api.Services.Interfaces;

public interface IConversationService
{
    Task<Conversation> CreateConversationAsync(string userId, string title);
    Task<Conversation?> GetConversationAsync(string conversationId, string userId);
    Task<List<Conversation>> GetUserConversationsAsync(string userId);
    Task<Conversation> AddMessageAsync(string conversationId, string userId, string role, string content);
    Task DeleteConversationAsync(string conversationId, string userId);
}