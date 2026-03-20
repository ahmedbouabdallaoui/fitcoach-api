using FitCoach.Api.Domain.Entities;
using FitCoach.Api.DTOs.Responses;

namespace FitCoach.Api.Services.Interfaces;

public interface ITrainingPlanService
{
    Task<ChatResponse> GenerateAsync(Conversation conversation, UserProfile profile, string userName);
}