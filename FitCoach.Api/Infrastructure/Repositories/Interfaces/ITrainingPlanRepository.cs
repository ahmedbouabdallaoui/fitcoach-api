using FitCoach.Api.Domain.Entities;

namespace FitCoach.Api.Infrastructure.Repositories.Interfaces;

// Contract for training plan data access.
public interface ITrainingPlanRepository
{
    Task<TrainingPlan?> GetByIdAsync(string id);
    Task<List<TrainingPlan>> GetByUserIdAsync(string userId);
    Task<TrainingPlan> CreateAsync(TrainingPlan plan);
}