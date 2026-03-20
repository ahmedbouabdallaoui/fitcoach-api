using FitCoach.Api.Domain.Entities;
using FitCoach.Api.Infrastructure.HttpClients.Responses;

namespace FitCoach.Api.Services.Interfaces;

public interface IGroqService
{
    Task<Dictionary<string, object?>> ExtractProfileDataAsync(string message, List<string> missingFields);
    Task<Dictionary<string, object?>> ExtractContextDataAsync(string message, List<string> missingFields, string tag);
    Task<string> GenerateProfileQuestionAsync(string userName, List<string> missingFields);
    Task<string> GenerateContextQuestionAsync(string userName, List<string> missingFields, string tag);
    Task<string> FormatGoalWarningAsync(string userName, string? warning, string recommendedGoal);
    Task<string> FormatTrainingPlanAsync(string userName, TrainingPlanMLResponse mlResponse);
    Task<string> FormatNutritionAdviceAsync(string userName, NutritionMLResponse mlResponse);
    Task<string> FormatInjuryReportAsync(string userName, InjuryPredictionMLResponse mlResponse);
    Task<string> GenerateRAGResponseAsync(string message, List<Message> history, UserProfile profile);
}