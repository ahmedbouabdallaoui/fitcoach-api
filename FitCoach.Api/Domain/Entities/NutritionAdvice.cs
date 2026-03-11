using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FitCoach.Api.Domain.Entities;

// Represents a generated nutrition plan for a user.
public class NutritionAdvice
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    public string UserId { get; set; } = string.Empty;

    public string Goal { get; set; } = string.Empty; // "weight_loss" | "muscle_gain" | "maintenance"

    // Calculated macros based on Harris-Benedict formula
    public int DailyCalories { get; set; }
    public int ProteinGrams { get; set; }
    public int CarbsGrams { get; set; }
    public int FatGrams { get; set; }

    public List<MealPlan> MealPlans { get; set; } = new();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}