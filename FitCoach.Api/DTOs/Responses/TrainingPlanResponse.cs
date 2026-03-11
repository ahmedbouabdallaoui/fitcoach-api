namespace FitCoach.Api.DTOs.Responses;

// Returned after generating a training plan.
public class TrainingPlanResponse
{
    public string Id { get; set; } = string.Empty;
    public string Goal { get; set; } = string.Empty;
    public string FitnessLevel { get; set; } = string.Empty;
    public int DaysPerWeek { get; set; }
    public int DurationWeeks { get; set; }
    public List<SeanceResponse> Seances { get; set; } = new();
    public DateTime CreatedAt { get; set; }
}
