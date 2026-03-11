using FitCoach.Api.Domain.Entities;
using FitCoach.Api.DTOs.Responses;

namespace FitCoach.Api.Mappers;

// Converts between InjuryPrediction entities and DTOs.
public static class InjuryPredictionMapper
{
    // Maps an InjuryPrediction entity to an InjuryPredictionResponse DTO
    public static InjuryPredictionResponse ToResponse(InjuryPrediction prediction)
    {
        return new InjuryPredictionResponse
        {
            Id = prediction.Id,
            RiskScore = prediction.RiskScore,
            RiskLevel = prediction.RiskLevel,
            Recommendations = prediction.Recommendations,
            CreatedAt = prediction.CreatedAt
        };
    }
}