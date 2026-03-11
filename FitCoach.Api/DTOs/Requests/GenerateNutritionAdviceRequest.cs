using System.ComponentModel.DataAnnotations;

namespace FitCoach.Api.DTOs.Requests;

// Data required to generate a personalized nutrition plan.
public class GenerateNutritionAdviceRequest
{
    [Required] public string Goal { get; set; } = string.Empty;          // "weight_loss" | "muscle_gain" | "maintenance"
    [Required] public double WeightKg { get; set; }
    [Required] public double HeightCm { get; set; }
    [Required] public int Age { get; set; }
    [Required] public string Gender { get; set; } = string.Empty;        // "male" | "female"
    [Required] public string ActivityLevel { get; set; } = string.Empty; // "sedentary" | "active" | "very_active"
}