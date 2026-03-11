namespace FitCoach.Api.DTOs.Responses;

// Returned after predicting injury risk for a user.
public class InjuryPredictionResponse
{
    public string Id { get; set; } = string.Empty;
    public double RiskScore { get; set; }    // 0 to 10
    public string RiskLevel { get; set; } = string.Empty;  // "low" | "medium" | "high"
    public string Recommendations { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}