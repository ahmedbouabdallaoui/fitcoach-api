using FitCoach.Api.Domain.Entities;
using FitCoach.Api.Services.Interfaces;

namespace FitCoach.Api.Services;

public class ProfileCompletenessChecker : IProfileCompletenessChecker
{
    public List<string> GetMissingProfileFields(UserProfile profile)
    {
        var missing = new List<string>();
        if (profile.Age == null) missing.Add("age");
        if (profile.WeightKg == null) missing.Add("weight");
        if (profile.HeightCm == null) missing.Add("height");
        if (profile.Gender == null) missing.Add("gender");
        if (profile.FitnessLevel == null) missing.Add("fitness_level");
        return missing;
    }

    public bool IsProfileComplete(UserProfile profile)
    {
        return GetMissingProfileFields(profile).Count == 0;
    }

    public List<string> GetMissingContextFields(ConversationContext context, string tag)
    {
        var missing = new List<string>();

        switch (tag.ToLower())
        {
            case "training":
                if (context.Goal == null) missing.Add("goal");
                if (context.DaysPerWeek == null) missing.Add("days_per_week");
                if (context.DurationWeeks == null) missing.Add("duration_weeks");
                break;

            case "nutrition":
                if (context.Goal == null) missing.Add("goal");
                if (context.ActivityLevel == null) missing.Add("activity_level");
                break;

            case "injury":
                if (context.WeeklyTrainingHours == null) missing.Add("weekly_training_hours");
                if (context.HasPreviousInjuries == null) missing.Add("previous_injuries");
                break;
        }

        return missing;
    }

    public bool IsContextReady(ConversationContext context, string tag)
    {
        return GetMissingContextFields(context, tag).Count == 0;
    }
}