namespace FitCoach.Api.DTOs.Responses;

// Represents a single meal inside a NutritionAdviceResponse.
public class MealPlanResponse
{
    public string MealType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Calories { get; set; }
}