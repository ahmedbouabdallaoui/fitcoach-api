using FitCoach.Api.Domain.Entities;
using System.Text.Json;

namespace FitCoach.Api.Mappers;

public static class ConversationContextMapper
{
    public static void UpdateFromExtracted(
        ConversationContext context,
        Dictionary<string, object?> extracted)
    {
        if (extracted.TryGetValue("goal", out var goal) && goal != null)
            context.Goal = GetString(goal);
        if (extracted.TryGetValue("days_per_week", out var days) && days != null)
            context.DaysPerWeek = GetInt(days);
        if (extracted.TryGetValue("duration_weeks", out var weeks) && weeks != null)
            context.DurationWeeks = GetInt(weeks);
        if (extracted.TryGetValue("activity_level", out var activity) && activity != null)
            context.ActivityLevel = GetString(activity);
        if (extracted.TryGetValue("weekly_training_hours", out var hours) && hours != null)
            context.WeeklyTrainingHours = GetDouble(hours);
        if (extracted.TryGetValue("previous_injuries", out var injuries) && injuries != null)
            context.HasPreviousInjuries = GetBool(injuries);
        if (extracted.TryGetValue("recent_symptoms", out var symptoms) && symptoms != null)
            context.RecentSymptoms = GetString(symptoms);
        if (extracted.TryGetValue("specific_days", out var specificDays) && specificDays != null)
            context.SpecificDays = GetStringList(specificDays);
    }

    private static string? GetString(object value)
    {
        if (value is JsonElement je)
            return je.ValueKind == JsonValueKind.String ? je.GetString() : je.ToString();
        return value?.ToString();
    }

    private static int? GetInt(object value)
    {
        if (value is JsonElement je)
        {
            if (je.ValueKind == JsonValueKind.Number) return je.GetInt32();
            if (int.TryParse(je.ToString(), out var i)) return i;
        }
        if (int.TryParse(value?.ToString(), out var result)) return result;
        return null;
    }

    private static double? GetDouble(object value)
    {
        if (value is JsonElement je)
        {
            if (je.ValueKind == JsonValueKind.Number) return je.GetDouble();
            if (double.TryParse(je.ToString(), out var d)) return d;
        }
        if (double.TryParse(value?.ToString(), out var result)) return result;
        return null;
    }

    private static bool? GetBool(object value)
    {
        if (value is JsonElement je)
        {
            if (je.ValueKind == JsonValueKind.True) return true;
            if (je.ValueKind == JsonValueKind.False) return false;
        }
        if (bool.TryParse(value?.ToString(), out var result)) return result;
        return null;
    }

    private static List<string>? GetStringList(object value)
    {
        if (value is JsonElement je && je.ValueKind == JsonValueKind.Array)
            return je.EnumerateArray().Select(e => e.GetString() ?? "").ToList();
        return null;
    }
}