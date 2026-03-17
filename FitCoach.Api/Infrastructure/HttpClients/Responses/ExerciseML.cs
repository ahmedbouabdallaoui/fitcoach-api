namespace FitCoach.Api.Infrastructure.HttpClients.Responses;
public class ExerciseML
{
    public string Name { get; set; } = string.Empty;
    public string BodyPart { get; set; } = string.Empty;
    public string Equipment { get; set; } = string.Empty;
    public string Level { get; set; } = string.Empty;
    public int Sets { get; set; }
    public int Reps { get; set; }
    public int RestSeconds { get; set; }
}