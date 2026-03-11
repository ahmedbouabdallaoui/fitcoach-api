using FitCoach.Api.Domain.Entities;
using FitCoach.Api.DTOs.Responses;

namespace FitCoach.Api.Mappers;

// Converts between EquipmentRecommendation entities and DTOs.
public static class EquipmentRecommendationMapper
{
    // Maps a list of EquipmentRecommendation entities to a list of response DTOs
    public static List<EquipmentRecommendationResponse> ToResponseList(
        List<EquipmentRecommendation> recommendations)
    {
        return recommendations.Select(r => new EquipmentRecommendationResponse
        {
            ProductId = r.ProductId,
            ProductName = r.ProductName,
            Reason = r.Reason,
            RelevanceScore = r.RelevanceScore
        }).ToList();
    }
}