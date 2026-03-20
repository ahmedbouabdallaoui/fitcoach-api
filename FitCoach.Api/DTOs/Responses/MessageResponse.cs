namespace FitCoach.Api.DTOs.Responses;

public class MessageResponse
{
    public string Role { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? Tag { get; set; }
    public DateTime Timestamp { get; set; }
}