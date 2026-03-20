using FitCoach.Api.Domain.Entities;
using FitCoach.Api.DTOs.Responses;

namespace FitCoach.Api.Mappers;

public static class ConversationMapper
{
    public static ConversationResponse ToResponse(Conversation conversation) => new()
    {
        Id = conversation.Id,
        Title = conversation.Title,
        Messages = conversation.Messages.Select(m => new MessageResponse
        {
            Role = m.Role,
            Content = m.Content,
            Tag = m.Tag,
            Timestamp = m.Timestamp
        }).ToList(),
        CreatedAt = conversation.CreatedAt,
        UpdatedAt = conversation.UpdatedAt
    };
}