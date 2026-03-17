using FitCoach.Api.Domain.Entities;
using FitCoach.Api.Infrastructure.Repositories.Interfaces;
using FitCoach.Api.Services.Interfaces;

namespace FitCoach.Api.Services;

public class ConversationService : IConversationService
{
    private readonly IConversationRepository _conversationRepository;
    private readonly ILogger<ConversationService> _logger;

    public ConversationService(
        IConversationRepository conversationRepository,
        ILogger<ConversationService> logger)
    {
        _conversationRepository = conversationRepository;
        _logger = logger;
    }

    public async Task<Conversation> CreateConversationAsync(string userId, string title)
    {
        var conversation = new Conversation
        {
            UserId = userId,
            Title = title,
            CreatedAt = DateTime.UtcNow,
            Messages = new List<Message>()
        };

        await _conversationRepository.CreateAsync(conversation);

        _logger.LogInformation(
            "Conversation created for user {UserId} with title {Title}",
            userId, title
        );

        return conversation;
    }

    public async Task<Conversation?> GetConversationAsync(string conversationId, string userId)
    {
        var conversation = await _conversationRepository.GetByIdAsync(conversationId);

        // Make sure user can only access their own conversations
        if (conversation == null || conversation.UserId != userId)
            return null;

        return conversation;
    }

    public async Task<List<Conversation>> GetUserConversationsAsync(string userId)
    {
        return await _conversationRepository.GetByUserIdAsync(userId);
    }

    public async Task<Conversation> AddMessageAsync(
        string conversationId,
        string userId,
        string role,
        string content)
    {
        var conversation = await GetConversationAsync(conversationId, userId);

        if (conversation == null)
            throw new Exception($"Conversation {conversationId} not found.");

        var message = new Message
        {
            Role = role,      // "user" or "assistant"
            Content = content,
            Timestamp = DateTime.UtcNow
        };

        conversation.Messages.Add(message);
        conversation.UpdatedAt = DateTime.UtcNow;

        await _conversationRepository.UpdateAsync(conversation);

        return conversation;
    }

    public async Task DeleteConversationAsync(string conversationId, string userId)
    {
        var conversation = await GetConversationAsync(conversationId, userId);

        if (conversation == null)
            throw new Exception($"Conversation {conversationId} not found.");

        await _conversationRepository.DeleteAsync(conversationId);

        _logger.LogInformation(
            "Conversation {ConversationId} deleted by user {UserId}",
            conversationId, userId
        );
    }
}