using FitCoach.Api.Domain.Entities;

namespace FitCoach.Api.Infrastructure.Repositories.Interfaces;

// Contract for equipment recommendation data access.
public interface IEquipmentRecommendationRepository
{
    Task<List<EquipmentRecommendation>> GetByUserIdAsync(string userId);
    Task<EquipmentRecommendation> CreateAsync(EquipmentRecommendation recommendation);
}