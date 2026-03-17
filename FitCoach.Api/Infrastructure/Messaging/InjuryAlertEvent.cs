namespace FitCoach.Api.Infrastructure.Messaging;

// This is the message that gets published to RabbitMQ
// Notification Service will deserialize this exact structure
public class InjuryAlertEvent
{
    public string UserId { get; set; } = string.Empty;
    public double RiskScore { get; set; }
    public string RiskLevel { get; set; } = string.Empty;
    public List<string> RiskFactors { get; set; } = new();
    public List<string> PreventionAdvice { get; set; } = new();
    public DateTime DetectedAt { get; set; } = DateTime.UtcNow;
    public string EventType { get; set; } = "HIGH_RISK_INJURY_DETECTED";
}