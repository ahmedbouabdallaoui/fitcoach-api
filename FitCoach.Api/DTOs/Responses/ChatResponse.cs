namespace FitCoach.Api.DTOs.Responses;

public class ChatResponse
{
    public string ConversationId { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;        // Groq formatted response
    public string MessageType { get; set; } = string.Empty;    // "question" / "plan" / "nutrition" / "injury" / "chat"
    public bool IsGenerating { get; set; } = false;            // true when ML result is included
    public List<EStoreProductCard>? ProductSuggestions { get; set; } // null for most responses
}

public class EStoreProductCard
{
    public string ProductId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Price { get; set; } = string.Empty;
    public double Rating { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
}