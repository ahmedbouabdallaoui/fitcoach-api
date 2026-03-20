using FitCoach.Api.Domain.Entities;
using FitCoach.Api.DTOs.Responses;
using FitCoach.Api.Infrastructure.HttpClients.Interfaces;
using FitCoach.Api.Infrastructure.Repositories.Interfaces;
using FitCoach.Api.Services.Interfaces;

namespace FitCoach.Api.Services;

public class NutritionService : INutritionService
{
    private readonly IMLServiceClient _mlServiceClient;
    private readonly INutritionAdviceRepository _nutritionAdviceRepository;
    private readonly IGroqService _groqService;
    private readonly ILogger<NutritionService> _logger;

    public NutritionService(
        IMLServiceClient mlServiceClient,
        INutritionAdviceRepository nutritionAdviceRepository,
        IGroqService groqService,
        ILogger<NutritionService> logger)
    {
        _mlServiceClient = mlServiceClient;
        _nutritionAdviceRepository = nutritionAdviceRepository;
        _groqService = groqService;
        _logger = logger;
    }

    public async Task<ChatResponse> GenerateAsync(
        Conversation conversation,
        UserProfile profile,
        string userName)
    {
        // --- Step 1: Calculate nutrition from ML ---
        var mlResponse = await _mlServiceClient.CalculateNutritionAsync(
            profile, conversation.Context
        );

        // --- Step 2: Format with Groq ---
        var formatted = await _groqService.FormatNutritionAdviceAsync(userName, mlResponse);

        // --- Step 3: Save to MongoDB ---
        var advice = new NutritionAdvice
        {
            UserId = profile.UserId,
            ConversationId = conversation.Id,
            Goal = conversation.Context.Goal!,
            TargetCalories = mlResponse.TargetCalories,
            Bmr = mlResponse.Bmr,
            Tdee = mlResponse.Tdee,
            FormattedAdvice = formatted,
            CreatedAt = DateTime.UtcNow
        };

        await _nutritionAdviceRepository.CreateAsync(advice);

        _logger.LogInformation(
            "Nutrition advice generated for user {UserId} goal {Goal}",
            profile.UserId, conversation.Context.Goal
        );

        return new ChatResponse
        {
            ConversationId = conversation.Id,
            Message = formatted,
            MessageType = "nutrition",
            IsGenerating = true
        };
    }
}