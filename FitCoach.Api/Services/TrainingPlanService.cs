using FitCoach.Api.Domain.Entities;
using FitCoach.Api.DTOs.Responses;
using FitCoach.Api.Infrastructure.HttpClients.Interfaces;
using FitCoach.Api.Infrastructure.Repositories.Interfaces;
using FitCoach.Api.Services.Interfaces;

namespace FitCoach.Api.Services;

public class TrainingPlanService : ITrainingPlanService
{
    private readonly IMLServiceClient _mlServiceClient;
    private readonly ITrainingPlanRepository _trainingPlanRepository;
    private readonly IGroqService _groqService;
    private readonly IProfileCompletenessChecker _profileChecker;
    private readonly ILogger<TrainingPlanService> _logger;

    public TrainingPlanService(
        IMLServiceClient mlServiceClient,
        ITrainingPlanRepository trainingPlanRepository,
        IGroqService groqService,
        IProfileCompletenessChecker profileChecker,
        ILogger<TrainingPlanService> logger)
    {
        _mlServiceClient = mlServiceClient;
        _trainingPlanRepository = trainingPlanRepository;
        _groqService = groqService;
        _profileChecker = profileChecker;
        _logger = logger;
    }

    public async Task<ChatResponse> GenerateAsync(
        Conversation conversation,
        UserProfile profile,
        string userName)
    {
        // --- Step 1: Validate goal ---
        var goalValidation = await _mlServiceClient.ValidateGoalAsync(
            profile, conversation.Context.Goal!
        );

        // --- Step 2: Reality check if goal unrealistic ---
        if (!goalValidation.GoalValid && conversation.Context.GoalOverridden != true)
        {
            var warning = await _groqService.FormatGoalWarningAsync(
                userName,
                goalValidation.WarningMessage,
                goalValidation.RecommendedGoal
            );

            // Mark as warned — next message user confirms or declines
            conversation.Context.GoalOverridden = false;

            return new ChatResponse
            {
                ConversationId = conversation.Id,
                Message = warning,
                MessageType = "question",
                IsGenerating = false
            };
        }

        // --- Step 3: Generate plan from ML ---
        var mlResponse = await _mlServiceClient.PredictTrainingPlanAsync(
            profile, conversation.Context
        );

        // --- Step 4: Format with Groq ---
        var formatted = await _groqService.FormatTrainingPlanAsync(userName, mlResponse);

        // --- Step 5: Save plan to MongoDB ---
        var plan = new TrainingPlan
        {
            UserId = profile.UserId,
            ConversationId = conversation.Id,
            Goal = conversation.Context.Goal!,
            FitnessLevel = profile.FitnessLevel ?? "beginner",
            DaysPerWeek = conversation.Context.DaysPerWeek ?? 3,
            DurationWeeks = conversation.Context.DurationWeeks ?? 4,
            PlanType = mlResponse.PlanType,
            FormattedPlan = formatted,
            CreatedAt = DateTime.UtcNow
        };

        await _trainingPlanRepository.CreateAsync(plan);

        _logger.LogInformation(
            "Training plan generated for user {UserId} goal {Goal}",
            profile.UserId, conversation.Context.Goal
        );

        return new ChatResponse
        {
            ConversationId = conversation.Id,
            Message = formatted,
            MessageType = "plan",
            IsGenerating = true
        };
    }
}