using FitCoach.Api.Domain.Entities;
using FitCoach.Api.Infrastructure.Repositories.Interfaces;
using FitCoach.Api.Mappers;
using FitCoach.Api.Services.Interfaces;

namespace FitCoach.Api.Services;

public class ProfileService : IProfileService
{
    private readonly IUserProfileRepository _profileRepository;
    private readonly IGroqService _groqService;
    private readonly IProfileCompletenessChecker _profileChecker;
    private readonly ILogger<ProfileService> _logger;

    public ProfileService(
        IUserProfileRepository profileRepository,
        IGroqService groqService,
        IProfileCompletenessChecker profileChecker,
        ILogger<ProfileService> logger)
    {
        _profileRepository = profileRepository;
        _groqService = groqService;
        _profileChecker = profileChecker;
        _logger = logger;
    }

    public async Task<UserProfile> GetOrCreateAsync(string userId)
    {
        var profile = await _profileRepository.GetByUserIdAsync(userId);

        if (profile == null)
        {
            profile = new UserProfile { UserId = userId };
            await _profileRepository.CreateAsync(profile);
            _logger.LogInformation("Created new profile for user {UserId}", userId);
        }

        return profile;
    }

    public async Task<UserProfile> UpdateFromMessageAsync(UserProfile profile, string message)
    {
        var missingFields = _profileChecker.GetMissingProfileFields(profile);

        if (missingFields.Count == 0)
            return profile;

        var extracted = await _groqService.ExtractProfileDataAsync(message, missingFields);
        UserProfileMapper.UpdateFromExtracted(profile, extracted);

        return profile;
    }

    public async Task SaveAsync(UserProfile profile)
    {
        profile.UpdatedAt = DateTime.UtcNow;
        await _profileRepository.UpdateAsync(profile);
        _logger.LogInformation("Profile saved for user {UserId}", profile.UserId);
    }
}