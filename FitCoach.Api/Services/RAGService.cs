using FitCoach.Api.Domain.Entities;
using FitCoach.Api.DTOs.Responses;
using FitCoach.Api.Services.Interfaces;

namespace FitCoach.Api.Services;

public class RAGService : IRAGService
{
    private readonly IGroqService _groqService;
    private readonly ILogger<RAGService> _logger;

    public RAGService(
        IGroqService groqService,
        ILogger<RAGService> logger)
    {
        _groqService = groqService;
        _logger = logger;
    }

    public async Task<ChatResponse> GenerateAsync(
        Conversation conversation,
        UserProfile profile,
        string message)
    {
        var response = await _groqService.GenerateRAGResponseAsync(
            message,
            conversation.Messages,
            profile
        );

        _logger.LogInformation(
            "RAG response generated for user {UserId}",
            profile.UserId
        );

        return new ChatResponse
        {
            ConversationId = conversation.Id,
            Message = response,
            MessageType = "chat",
            IsGenerating = false
        };
    }
}