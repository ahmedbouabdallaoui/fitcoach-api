namespace FitCoach.Api.Domain.Entities;

// Represents a single workout session inside a TrainingPlan.
public class Seance
{
    public int DayNumber { get; set; }

    public string MuscleGroup { get; set; } = string.Empty;

    public List<Exercice> Exercices { get; set; } = new();
}