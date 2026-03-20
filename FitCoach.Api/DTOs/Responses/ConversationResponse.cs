namespace FitCoach.Api.DTOs.Responses;

// Returned after sending a message — contains the AI reply and conversation context.
public class ConversationResponse
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public List<MessageResponse> Messages { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}