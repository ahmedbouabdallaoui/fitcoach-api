using FitCoach.Api.DTOs.Requests;
using FitCoach.Api.Domain.Entities;
using FitCoach.Api.DTOs.Responses;

namespace FitCoach.Api.Services.Interfaces;

public interface IChatService
{
    Task<ChatResponse> ProcessMessageAsync(ChatRequest request, string userId, string userName);
}