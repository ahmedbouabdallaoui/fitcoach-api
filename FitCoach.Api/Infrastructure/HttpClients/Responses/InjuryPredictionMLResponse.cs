namespace FitCoach.Api.Infrastructure.HttpClients.Responses;

public class InjuryPredictionMLResponse
{
    public double RiskScore { get; set; }        // 0-10 scale
    public string RiskLevel { get; set; } = string.Empty;  // "Low", "Medium", "High"
    public bool HighRisk { get; set; }           // true if score >= 7
    public List<string> RiskFactors { get; set; } = new();
    public List<string> PreventionAdvice { get; set; } = new();
    public string FormattedReport { get; set; } = string.Empty; // Groq formatted output
}