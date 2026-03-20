namespace FitCoach.Api.Domain.Entities;

public class ConversationContext
{
    // Training plan fields
    public string? Goal { get; set; }           // "weight_loss" / "muscle_gain" / "endurance" / "maintenance"
    public int? DaysPerWeek { get; set; }
    public int? DurationWeeks { get; set; }
    public bool? GoalOverridden { get; set; }   // true if user insisted on unrealistic goal

    // Nutrition fields
    public string? ActivityLevel { get; set; }  // "sedentary" / "light" / "moderate" / "active" / "very_active"

    // Injury fields
    public double? WeeklyTrainingHours { get; set; }
    public bool? HasPreviousInjuries { get; set; }
    public string? RecentSymptoms { get; set; }

    // State
    public bool IsReadyForGeneration { get; set; } = false;
}