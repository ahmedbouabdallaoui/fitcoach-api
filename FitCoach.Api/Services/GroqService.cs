using FitCoach.Api.Domain.Entities;
using FitCoach.Api.Infrastructure.HttpClients.Interfaces;
using FitCoach.Api.Infrastructure.HttpClients.Responses;
using FitCoach.Api.Services.Interfaces;

namespace FitCoach.Api.Services;

public class GroqService : IGroqService
{
    private readonly IMLServiceClient _mlServiceClient;

    public GroqService(IMLServiceClient mlServiceClient)
    {
        _mlServiceClient = mlServiceClient;
    }

    public async Task<Dictionary<string, object?>> ExtractProfileDataAsync(string message, List<string> missingFields)
        => await _mlServiceClient.ExtractProfileDataAsync(message, missingFields);

    public async Task<Dictionary<string, object?>> ExtractContextDataAsync(string message, List<string> missingFields, string tag)
        => await _mlServiceClient.ExtractContextDataAsync(message, missingFields, tag);

    public async Task<string> GenerateProfileQuestionAsync(string userName, List<string> missingFields)
        => await _mlServiceClient.GenerateProfileQuestionAsync(userName, missingFields);

    public async Task<string> GenerateContextQuestionAsync(string userName, List<string> missingFields, string tag)
        => await _mlServiceClient.GenerateContextQuestionAsync(userName, missingFields, tag);

    public async Task<string> FormatGoalWarningAsync(string userName, string? warning, string recommendedGoal)
        => await _mlServiceClient.FormatGoalWarningAsync(userName, warning, recommendedGoal);

    public async Task<string> FormatTrainingPlanAsync(string userName, TrainingPlanMLResponse mlResponse)
        => Task.FromResult(mlResponse.FormattedPlan).Result; // already formatted by Python

    public async Task<string> FormatNutritionAdviceAsync(string userName, NutritionMLResponse mlResponse)
        => Task.FromResult(mlResponse.FormattedAdvice).Result; // already formatted by Python

    public async Task<string> FormatInjuryReportAsync(string userName, InjuryPredictionMLResponse mlResponse)
        => Task.FromResult(mlResponse.FormattedReport).Result; // already formatted by Python

    public async Task<string> GenerateRAGResponseAsync(string message, List<Message> history, UserProfile profile)
        => await _mlServiceClient.GenerateRAGResponseAsync(message, history, profile);
}