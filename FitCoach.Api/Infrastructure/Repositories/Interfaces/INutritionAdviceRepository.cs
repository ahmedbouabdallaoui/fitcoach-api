using FitCoach.Api.Domain.Entities;

namespace FitCoach.Api.Infrastructure.Repositories.Interfaces;

// Contract for nutrition advice data access.
public interface INutritionAdviceRepository
{
    Task<NutritionAdvice?> GetByIdAsync(string id);
    Task<List<NutritionAdvice>> GetByUserIdAsync(string userId);
    Task<NutritionAdvice> CreateAsync(NutritionAdvice advice);
}