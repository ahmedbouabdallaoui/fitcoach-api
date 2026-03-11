using FitCoach.Api.Domain.Entities;

namespace FitCoach.Api.Infrastructure.Repositories.Interfaces;

// Contract for injury prediction data access.
public interface IInjuryPredictionRepository
{
    Task<InjuryPrediction?> GetByIdAsync(string id);
    Task<List<InjuryPrediction>> GetByUserIdAsync(string userId);
    Task<InjuryPrediction> CreateAsync(InjuryPrediction prediction);
}