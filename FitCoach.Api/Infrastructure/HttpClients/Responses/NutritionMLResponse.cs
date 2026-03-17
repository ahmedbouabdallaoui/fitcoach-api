namespace FitCoach.Api.Infrastructure.HttpClients.Responses;

public class NutritionMLResponse
{
    public double Bmr { get; set; }
    public double Tdee { get; set; }
    public double TargetCalories { get; set; }
    public MacroML Macros { get; set; } = new();
    public string Goal { get; set; } = string.Empty;
    public List<MealML> MealStructure { get; set; } = new();
    public List<string> SupplementSuggestions { get; set; } = new();
    public string FormattedAdvice { get; set; } = string.Empty; // Groq formatted output
}
