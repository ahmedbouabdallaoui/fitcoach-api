using FitCoach.Api.Domain.Entities;

namespace FitCoach.Api.Mappers;

public static class UserProfileMapper
{
    public static void UpdateFromExtracted(
        UserProfile profile,
        Dictionary<string, object?> extracted)
    {
        if (extracted.TryGetValue("age", out var age) && age != null)
            profile.Age = Convert.ToInt32(age);
        if (extracted.TryGetValue("weight", out var weight) && weight != null)
            profile.WeightKg = Convert.ToDouble(weight);
        if (extracted.TryGetValue("height", out var height) && height != null)
            profile.HeightCm = Convert.ToDouble(height);
        if (extracted.TryGetValue("gender", out var gender) && gender != null)
            profile.Gender = gender.ToString();
        if (extracted.TryGetValue("fitness_level", out var fitnessLevel) && fitnessLevel != null)
            profile.FitnessLevel = fitnessLevel.ToString();
        if (extracted.TryGetValue("body_fat_percentage", out var bfp) && bfp != null)
            profile.BodyFatPercentage = Convert.ToDouble(bfp);
    }
}