using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FitCoach.Api.Domain.Entities;

// Represents a generated workout plan for a user.
public class TrainingPlan
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    public string UserId { get; set; } = string.Empty;

    public string Goal { get; set; } = string.Empty; // "weight_loss" | "muscle_gain" | "endurance"

    public string FitnessLevel { get; set; } = string.Empty; // "beginner" | "intermediate" | "advanced"

    public int DaysPerWeek { get; set; }

    public int DurationWeeks { get; set; }

    public List<Seance> Seances { get; set; } = new();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}