using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FitCoach.Api.Domain.Entities;

// Represents an equipment recommendation for a user
// based on their training history and goals.
public class EquipmentRecommendation
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    public string UserId { get; set; } = string.Empty;

    public string ProductId { get; set; } = string.Empty;  // References E-Store product

    public string ProductName { get; set; } = string.Empty;

    public string Reason { get; set; } = string.Empty;  // Why this was recommended

    public double RelevanceScore { get; set; }  // 0 to 1

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}