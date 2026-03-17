using System.ComponentModel.DataAnnotations;

namespace FitCoach.Api.DTOs.Requests;

// Data required to generate a personalized training plan.
public class GenerateTrainingPlanRequest
{
    [Required] public string Goal { get; set; } = string.Empty;         // "weight_loss" | "muscle_gain" | "endurance"
    [Required] public string FitnessLevel { get; set; } = string.Empty; // "beginner" | "intermediate" | "advanced"
    [Required] public int DaysPerWeek { get; set; }
    [Required] public int DurationWeeks { get; set; }
    [Required] public double WeightKg { get; set; }
    [Required] public double HeightCm { get; set; }
    [Required] public int Age { get; set; }
    [Required] public string Gender { get; set; } = string.Empty; // "Male" or "Female"

}