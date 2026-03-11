using System.ComponentModel.DataAnnotations;

namespace FitCoach.Api.DTOs.Requests;

// Data required to send a message to the AI assistant.
public class ChatMessageRequest
{
    [Required]
    [MaxLength(1000)]
    public string Message { get; set; } = string.Empty;

    // Null means start a new conversation
    public string? ConversationId { get; set; }
}