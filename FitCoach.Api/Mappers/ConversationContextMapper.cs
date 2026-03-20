using FitCoach.Api.Domain.Entities;

namespace FitCoach.Api.Mappers;

public static class ConversationContextMapper
{
    public static void UpdateFromExtracted(
        ConversationContext context,
        Dictionary<string, object?> extracted)
    {
        if (extracted.TryGetValue("goal", out var goal) && goal != null)
            context.Goal = goal.ToString();
        if (extracted.TryGetValue("days_per_week", out var days) && days != null)
            context.DaysPerWeek = Convert.ToInt32(days);
        if (extracted.TryGetValue("duration_weeks", out var weeks) && weeks != null)
            context.DurationWeeks = Convert.ToInt32(weeks);
        if (extracted.TryGetValue("activity_level", out var activity) && activity != null)
            context.ActivityLevel = activity.ToString();
        if (extracted.TryGetValue("weekly_training_hours", out var hours) && hours != null)
            context.WeeklyTrainingHours = Convert.ToDouble(hours);
        if (extracted.TryGetValue("previous_injuries", out var injuries) && injuries != null)
            context.HasPreviousInjuries = Convert.ToBoolean(injuries);
        if (extracted.TryGetValue("recent_symptoms", out var symptoms) && symptoms != null)
            context.RecentSymptoms = symptoms.ToString();
    }
}