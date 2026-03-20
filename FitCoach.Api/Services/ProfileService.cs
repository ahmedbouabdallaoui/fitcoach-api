using FitCoach.Api.Domain.Entities;
using FitCoach.Api.Infrastructure.Repositories.Interfaces;
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

        // Ask Groq to extract any profile data from the message
        var extracted = await _groqService.ExtractProfileDataAsync(message, missingFields);

        if (extracted.TryGetValue("age", out var age) && age != null)
            profile.Age = Convert.ToInt32(age);
        if (extracted.TryGetValue("weight", out var weight) && weight != null)
            profile.WeightKg = Convert.ToDouble(weight);
        if (extracted.TryGetValue("height", out var height) && height != null)
            profile.HeightCm = Convert.ToDouble(height);
        if (extracted.TryGetValue("gender", out var gender) && gender != null)
            profile.Gender = gender.ToString();
        if (extracted.TryGetValue("fitness_level", out var fitnessLevel) && fitnessLevel != null)
            profile.FitnessLevel = fitnessLevel.ToString();
        if (extracted.TryGetValue("body_fat_percentage", out var bfp) && bfp != null)
            profile.BodyFatPercentage = Convert.ToDouble(bfp);

        return profile;
    }

    public async Task SaveAsync(UserProfile profile)
    {
        profile.UpdatedAt = DateTime.UtcNow;
        await _profileRepository.UpdateAsync(profile);
        _logger.LogInformation("Profile saved for user {UserId}", profile.UserId);
    }
}