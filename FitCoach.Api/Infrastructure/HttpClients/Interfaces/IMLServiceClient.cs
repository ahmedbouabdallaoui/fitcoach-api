using FitCoach.Api.Domain.Entities;
using FitCoach.Api.Infrastructure.HttpClients.Responses;

namespace FitCoach.Api.Infrastructure.HttpClients.Interfaces;

public interface IMLServiceClient
{
    // --- ML Predictions ---
    Task<GoalValidationMLResponse> ValidateGoalAsync(UserProfile profile, string goal);
    Task<TrainingPlanMLResponse> PredictTrainingPlanAsync(UserProfile profile, ConversationContext context);
    Task<InjuryPredictionMLResponse> PredictInjuryRiskAsync(UserProfile profile, ConversationContext context);
    Task<NutritionMLResponse> CalculateNutritionAsync(UserProfile profile, ConversationContext context);

    // --- Groq via Python ---
    Task<string> GenerateProfileQuestionAsync(string userName, List<string> missingFields);
    Task<string> GenerateContextQuestionAsync(string userName, List<string> missingFields, string tag);
    Task<Dictionary<string, object?>> ExtractProfileDataAsync(string message, List<string> missingFields);
    Task<Dictionary<string, object?>> ExtractContextDataAsync(string message, List<string> missingFields, string tag);
    Task<string> GenerateRAGResponseAsync(string message, List<Message> history, UserProfile profile);
    Task<string> FormatGoalWarningAsync(string userName, string? warning, string recommendedGoal);
}