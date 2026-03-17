namespace FitCoach.Api.Infrastructure.HttpClients.Responses;

public class ExerciseDayML
{
    public string Day { get; set; } = string.Empty;
    public string WorkoutType { get; set; } = string.Empty;
    public int DurationMinutes { get; set; }
    public string Intensity { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
    public List<ExerciseML> Exercises { get; set; } = new();
}
