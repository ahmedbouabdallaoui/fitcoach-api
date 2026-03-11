namespace FitCoach.Api.DTOs.Responses;

// Returned after sending a message — contains the AI reply and conversation context.
public class ConversationResponse
{
    public string ConversationId { get; set; } = string.Empty;
    public string Reply { get; set; } = string.Empty;
    public DateTime UpdatedAt { get; set; }
}