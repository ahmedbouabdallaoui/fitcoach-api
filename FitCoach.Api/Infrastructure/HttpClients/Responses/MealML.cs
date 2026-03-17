namespace FitCoach.Api.Infrastructure.HttpClients.Responses;

public class MealML
{
    public string Meal { get; set; } = string.Empty;
    public string Timing { get; set; } = string.Empty;
    public int Calories { get; set; }
    public string Focus { get; set; } = string.Empty;
}
