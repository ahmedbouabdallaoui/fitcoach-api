using FitCoach.Api.Domain.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Conversation
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    public string UserId { get; set; } = string.Empty;

    public List<Message> Messages { get; set; } = new();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public string Title { get; set; } = "New Conversation";
}