namespace FitCoach.Api.Infrastructure.HttpClients.Responses;

public class TrainingPlanMLResponse
{
    public string PlanType { get; set; } = string.Empty;      // "normal", "overweight", "obese" etc.
    public string Goal { get; set; } = string.Empty;
    public string FormattedPlan { get; set; } = string.Empty; // Groq formatted output
    public List<ExerciseDayML> Days { get; set; } = new();
}
