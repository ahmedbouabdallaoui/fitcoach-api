using FitCoach.Api.Domain.Entities;

namespace FitCoach.Api.Services.Interfaces;

public interface IProfileService
{
    Task<UserProfile> GetOrCreateAsync(string userId);
    Task<UserProfile> UpdateFromMessageAsync(UserProfile profile, string message);
    Task SaveAsync(UserProfile profile);
}