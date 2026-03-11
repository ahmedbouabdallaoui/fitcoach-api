namespace FitCoach.Api.DTOs.Responses;

public class ExerciceResponse
{
    public string Name { get; set; } = string.Empty;
    public int Sets { get; set; }
    public int Reps { get; set; }
    public int RestSeconds { get; set; }
}