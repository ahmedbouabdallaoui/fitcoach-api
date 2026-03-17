namespace FitCoach.Api.Infrastructure.HttpClients.Responses;

public class GoalValidationMLResponse
{
    public string RealisticGoal { get; set; } = string.Empty;
    public bool GoalValid { get; set; }
    public string UserGoal { get; set; } = string.Empty;
    public string? WarningMessage { get; set; }
}