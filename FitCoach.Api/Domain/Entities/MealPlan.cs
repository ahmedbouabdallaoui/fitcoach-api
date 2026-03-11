namespace FitCoach.Api.Domain.Entities;

// Represents a single day meal plan inside a NutritionAdvice.
public class MealPlan
{
    public string MealType { get; set; } = string.Empty; // "breakfast" | "lunch" | "dinner" | "snack"

    public string Description { get; set; } = string.Empty;

    public int Calories { get; set; }
}