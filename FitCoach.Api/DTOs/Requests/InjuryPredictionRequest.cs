using System.ComponentModel.DataAnnotations;

namespace FitCoach.Api.DTOs.Requests;

// Data required to predict injury risk for a user.
public class InjuryPredictionRequest
{
    [Required] public double WeightKg { get; set; }
    [Required] public double HeightCm { get; set; }
    [Required] public int Age { get; set; }
    [Required] public int WeeklyTrainingHours { get; set; }
    [Required] public string FitnessLevel { get; set; } = string.Empty; // "beginner" | "intermediate" | "advanced"
}