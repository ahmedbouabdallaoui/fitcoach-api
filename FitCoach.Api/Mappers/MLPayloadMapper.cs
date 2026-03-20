using FitCoach.Api.Domain.Entities;

namespace FitCoach.Api.Mappers;

public static class MLPayloadMapper
{
    public static object ToGoalValidationPayload(UserProfile profile, string goal) => new
    {
        user_id = profile.UserId,
        age = profile.Age,
        weight_kg = profile.WeightKg,
        height_cm = profile.HeightCm,
        gender = profile.Gender,
        fitness_level = profile.FitnessLevel,
        goal
    };

    public static object ToTrainingPlanPayload(UserProfile profile, ConversationContext context) => new
    {
        user_id = profile.UserId,
        age = profile.Age,
        weight_kg = profile.WeightKg,
        height_cm = profile.HeightCm,
        gender = profile.Gender,
        fitness_level = profile.FitnessLevel,
        goal = context.Goal,
        days_per_week = context.DaysPerWeek,
        duration_weeks = context.DurationWeeks
    };

    public static object ToInjuryPredictionPayload(UserProfile profile, ConversationContext context) => new
    {
        user_id = profile.UserId,
        age = profile.Age,
        weight_kg = profile.WeightKg,
        height_cm = profile.HeightCm,
        weekly_training_hours = context.WeeklyTrainingHours,
        fitness_level = profile.FitnessLevel,
        has_previous_injuries = context.HasPreviousInjuries,
        recent_symptoms = context.RecentSymptoms
    };

    public static object ToNutritionPayload(UserProfile profile, ConversationContext context) => new
    {
        user_id = profile.UserId,
        age = profile.Age,
        weight_kg = profile.WeightKg,
        height_cm = profile.HeightCm,
        gender = profile.Gender,
        activity_level = context.ActivityLevel,
        goal = context.Goal
    };
}