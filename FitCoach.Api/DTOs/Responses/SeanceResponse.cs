namespace FitCoach.Api.DTOs.Responses;

public class SeanceResponse
{
    public int DayNumber { get; set; }
    public string MuscleGroup { get; set; } = string.Empty;
    public List<ExerciceResponse> Exercices { get; set; } = new();
}