using System.ComponentModel.DataAnnotations;

namespace FitCoach.Api.DTOs.Requests;

public class ChatRequest
{
    [Required]
    [MaxLength(2000)]
    public string Message { get; set; } = string.Empty;

    // null = new conversation
    public string? ConversationId { get; set; }

    // null = general RAG chat
    // "training" / "nutrition" / "injury"
    public string? Tag { get; set; }
}