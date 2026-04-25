using System.Text.Json;
using FitCoach.Api.Domain.Entities;

namespace FitCoach.Api.Mappers;

public static class UserProfileMapper
{
    public static void UpdateFromExtracted(
        UserProfile profile,
        Dictionary<string, object?> extracted)
    {
        if (extracted.TryGetValue("age", out var age) && age != null)
            profile.Age = GetInt(age);
        if (extracted.TryGetValue("weight", out var weight) && weight != null)
            profile.WeightKg = GetDouble(weight);
        if (extracted.TryGetValue("height", out var height) && height != null)
            profile.HeightCm = GetDouble(height);
        if (extracted.TryGetValue("gender", out var gender) && gender != null)
            profile.Gender = GetString(gender);
        if (extracted.TryGetValue("fitness_level", out var fitnessLevel) && fitnessLevel != null)
            profile.FitnessLevel = GetString(fitnessLevel);
        if (extracted.TryGetValue("body_fat_percentage", out var bfp) && bfp != null)
            profile.BodyFatPercentage = GetDouble(bfp);
    }

    private static string? GetString(object value)
    {
        if (value is JsonElement je)
            return je.ValueKind == JsonValueKind.String ? je.GetString() : je.ToString();
        return value.ToString();
    }

    private static int? GetInt(object value)
    {
        if (value is JsonElement je)
        {
            if (je.ValueKind == JsonValueKind.Number) return je.GetInt32();
            if (int.TryParse(je.ToString(), out var i)) return i;
        }
        if (int.TryParse(value.ToString(), out var result)) return result;
        return null;
    }

    private static double? GetDouble(object value)
    {
        if (value is JsonElement je)
        {
            if (je.ValueKind == JsonValueKind.Number) return je.GetDouble();
            if (double.TryParse(je.ToString(), out var d)) return d;
        }
        if (double.TryParse(value.ToString(), out var result)) return result;
        return null;
    }
}