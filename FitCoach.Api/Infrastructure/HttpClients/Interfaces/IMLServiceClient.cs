using FitCoach.Api.DTOs.Requests;
using FitCoach.Api.Infrastructure.HttpClients.Responses;

namespace FitCoach.Api.Infrastructure.HttpClients.Interfaces;

public interface IMLServiceClient
{
    Task<GoalValidationMLResponse> ValidateGoalAsync(GenerateTrainingPlanRequest request, string userId);
    Task<TrainingPlanMLResponse> PredictTrainingPlanAsync(GenerateTrainingPlanRequest request, string userId);
    Task<InjuryPredictionMLResponse> PredictInjuryRiskAsync(InjuryPredictionRequest request, string userId);
    Task<NutritionMLResponse> CalculateNutritionAsync(GenerateNutritionAdviceRequest request, string userId);
}