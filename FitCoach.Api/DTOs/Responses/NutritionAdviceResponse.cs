namespace FitCoach.Api.DTOs.Responses;

// Returned after generating a nutrition plan.
public class NutritionAdviceResponse
{
    public string Id { get; set; } = string.Empty;
    public string Goal { get; set; } = string.Empty;
    public int DailyCalories { get; set; }
    public int ProteinGrams { get; set; }
    public int CarbsGrams { get; set; }
    public int FatGrams { get; set; }
    public List<MealPlanResponse> MealPlans { get; set; } = new();
    public DateTime CreatedAt { get; set; }
}
