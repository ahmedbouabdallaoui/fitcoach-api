using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FitCoach.Api.Domain.Entities;

public class UserProfile
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    public string UserId { get; set; } = string.Empty;

    // Physical data
    public int? Age { get; set; }
    public double? WeightKg { get; set; }
    public double? HeightCm { get; set; }
    public string? Gender { get; set; }              // "Male" / "Female"
    public double? BodyFatPercentage { get; set; }   // optional — user can skip
    public string? FitnessLevel { get; set; }        // "beginner" / "intermediate" / "advanced"

    // Metadata
    public bool IsComplete { get; set; } = false;    // true when all required fields filled
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Required fields — BFP is optional
}