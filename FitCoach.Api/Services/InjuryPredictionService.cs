using FitCoach.Api.Domain.Entities;
using FitCoach.Api.DTOs.Responses;
using FitCoach.Api.Infrastructure.HttpClients.Interfaces;
using FitCoach.Api.Infrastructure.Messaging;
using FitCoach.Api.Infrastructure.Repositories.Interfaces;
using FitCoach.Api.Services.Interfaces;

namespace FitCoach.Api.Services;

public class InjuryPredictionService : IInjuryPredictionService
{
    private readonly IMLServiceClient _mlServiceClient;
    private readonly IInjuryPredictionRepository _injuryPredictionRepository;
    private readonly IInjuryAlertPublisher _injuryAlertPublisher;
    private readonly IGroqService _groqService;
    private readonly ILogger<InjuryPredictionService> _logger;

    public InjuryPredictionService(
        IMLServiceClient mlServiceClient,
        IInjuryPredictionRepository injuryPredictionRepository,
        IInjuryAlertPublisher injuryAlertPublisher,
        IGroqService groqService,
        ILogger<InjuryPredictionService> logger)
    {
        _mlServiceClient = mlServiceClient;
        _injuryPredictionRepository = injuryPredictionRepository;
        _injuryAlertPublisher = injuryAlertPublisher;
        _groqService = groqService;
        _logger = logger;
    }

    public async Task<ChatResponse> GenerateAsync(
        Conversation conversation,
        UserProfile profile,
        string userId,
        string userName)
    {
        // --- Step 1: Run injury prediction ML ---
        var mlResponse = await _mlServiceClient.PredictInjuryRiskAsync(
            profile, conversation.Context
        );

        // --- Step 2: Format with Groq ---
        var formatted = await _groqService.FormatInjuryReportAsync(userName, mlResponse);

        // --- Step 3: Save to MongoDB ---
        var prediction = new InjuryPrediction
        {
            UserId = userId,
            ConversationId = conversation.Id,
            RiskScore = mlResponse.RiskScore,
            RiskLevel = mlResponse.RiskLevel,
            RiskFactors = mlResponse.RiskFactors,
            PreventionAdvice = mlResponse.PreventionAdvice,
            FormattedReport = formatted,
            CreatedAt = DateTime.UtcNow
        };

        await _injuryPredictionRepository.CreateAsync(prediction);

        // --- Step 4: Publish RabbitMQ alert if high risk ---
        if (mlResponse.HighRisk)
        {
            await _injuryAlertPublisher.PublishAsync(new InjuryAlertEvent
            {
                UserId = userId,
                RiskScore = mlResponse.RiskScore,
                RiskLevel = mlResponse.RiskLevel,
                RiskFactors = mlResponse.RiskFactors,
                PreventionAdvice = mlResponse.PreventionAdvice
            });

            _logger.LogWarning(
                "High injury risk detected for user {UserId} score {RiskScore}",
                userId, mlResponse.RiskScore
            );
        }

        return new ChatResponse
        {
            ConversationId = conversation.Id,
            Message = formatted,
            MessageType = "injury",
            IsGenerating = true
        };
    }
}