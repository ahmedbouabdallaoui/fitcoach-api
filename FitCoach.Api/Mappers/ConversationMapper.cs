using FitCoach.Api.Domain.Entities;
using FitCoach.Api.DTOs.Responses;

namespace FitCoach.Api.Mappers;

// Converts between Conversation entities and DTOs.
// Services use this — controllers never touch entities directly.
public static class ConversationMapper
{
    // Maps a Conversation entity to a ConversationResponse DTO
    public static ConversationResponse ToResponse(Conversation conversation, string reply)
    {
        return new ConversationResponse
        {
            ConversationId = conversation.Id,
            Reply = reply,
            UpdatedAt = conversation.UpdatedAt
        };
    }
}