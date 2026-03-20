using FitCoach.Api.Domain.Entities;
using FitCoach.Api.Infrastructure.HttpClients.Responses;
using FitCoach.Api.Services.Interfaces;

namespace FitCoach.Api.Services;

public class GroqService : IGroqService
{
    public Task<Dictionary<string, object?>> ExtractProfileDataAsync(string message, List<string> missingFields)
        => Task.FromResult(new Dictionary<string, object?>());

    public Task<Dictionary<string, object?>> ExtractContextDataAsync(string message, List<string> missingFields, string tag)
        => Task.FromResult(new Dictionary<string, object?>());

    public Task<string> GenerateProfileQuestionAsync(string userName, List<string> missingFields)
        => Task.FromResult(string.Empty);

    public Task<string> GenerateContextQuestionAsync(string userName, List<string> missingFields, string tag)
        => Task.FromResult(string.Empty);

    public Task<string> FormatGoalWarningAsync(string userName, string? warning, string recommendedGoal)
        => Task.FromResult(string.Empty);

    public Task<string> FormatTrainingPlanAsync(string userName, TrainingPlanMLResponse mlResponse)
        => Task.FromResult(string.Empty);

    public Task<string> FormatNutritionAdviceAsync(string userName, NutritionMLResponse mlResponse)
        => Task.FromResult(string.Empty);

    public Task<string> FormatInjuryReportAsync(string userName, InjuryPredictionMLResponse mlResponse)
        => Task.FromResult(string.Empty);

    public Task<string> GenerateRAGResponseAsync(string message, List<Message> history, UserProfile profile)
        => Task.FromResult(string.Empty);
}