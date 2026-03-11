namespace FitCoach.Api.Domain.Entities;

// Represents a single message in a conversation.
// Always accessed through its parent Conversation — never fetched in isolation.
public class Message
{
    public string Role { get; set; } = string.Empty;  // "user" or "assistant"

    public string Content { get; set; } = string.Empty;

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}