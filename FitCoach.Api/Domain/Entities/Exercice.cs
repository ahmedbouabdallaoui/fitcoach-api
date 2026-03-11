namespace FitCoach.Api.Domain.Entities;

// Represents a single exercise inside a Seance.
public class Exercice
{
    public string Name { get; set; } = string.Empty;

    public int Sets { get; set; }

    public int Reps { get; set; }

    public int RestSeconds { get; set; }
}