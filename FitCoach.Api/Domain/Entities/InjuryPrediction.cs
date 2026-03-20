using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FitCoach.Api.Domain.Entities;

// Represents an injury risk assessment for a user.
// If risk score >= 7, an event is published to RabbitMQ to notify the user.
public class InjuryPrediction
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
    public string UserId { get; set; } = string.Empty;
    public double RiskScore { get; set; }  // 0 to 10
    public string RiskLevel { get; set; } = string.Empty;  // "low" | "medium" | "high"
    public string Recommendations { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string ConversationId { get; set; } = string.Empty;
    public List<string> RiskFactors { get; set; } = new();
    public List<string> PreventionAdvice { get; set; } = new();
    public string FormattedReport { get; set; } = string.Empty;
}