using FitCoach.Api.Domain.Entities;
using FitCoach.Api.Infrastructure.HttpClients.Responses;

namespace FitCoach.Api.Infrastructure.HttpClients.Interfaces;

public interface IMLServiceClient
{
    Task<GoalValidationMLResponse> ValidateGoalAsync(UserProfile profile, string goal);
    Task<TrainingPlanMLResponse> PredictTrainingPlanAsync(UserProfile profile, ConversationContext context);
    Task<InjuryPredictionMLResponse> PredictInjuryRiskAsync(UserProfile profile, ConversationContext context);
    Task<NutritionMLResponse> CalculateNutritionAsync(UserProfile profile, ConversationContext context);
}