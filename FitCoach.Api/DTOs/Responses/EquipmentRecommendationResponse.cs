namespace FitCoach.Api.DTOs.Responses;

// Returned after generating equipment recommendations for a user.
public class EquipmentRecommendationResponse
{
    public string ProductId { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public double RelevanceScore { get; set; }  // 0 to 1
}