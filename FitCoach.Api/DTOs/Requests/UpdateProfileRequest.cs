namespace FitCoach.Api.DTOs.Requests;

public class UpdateProfileRequest
{
    public int? Age { get; set; }
    public double? WeightKg { get; set; }
    public double? HeightCm { get; set; }
    public string? Gender { get; set; }
    public string? FitnessLevel { get; set; }
    public double? BodyFatPercentage { get; set; }
}