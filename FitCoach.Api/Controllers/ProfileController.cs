using FitCoach.Api.Domain.Entities;
using FitCoach.Api.Security;
using FitCoach.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitCoach.Api.Controllers;

[ApiController]
[Route("api/profile")]
[Authorize]
public class ProfileController : ControllerBase
{
    private readonly IProfileService _profileService;
    private readonly ILogger<ProfileController> _logger;

    public ProfileController(
        IProfileService profileService,
        ILogger<ProfileController> logger)
    {
        _profileService = profileService;
        _logger = logger;
    }
    
    [HttpGet]
    public async Task<ActionResult<UserProfile>> GetProfile()
    {
        var userId = HttpContext.User.GetUserId();

        if (string.IsNullOrEmpty(userId))
            return Unauthorized("User ID not found in token.");

        var profile = await _profileService.GetOrCreateAsync(userId);
        return Ok(profile);
    }
    
    [HttpPut]
    public async Task<ActionResult<UserProfile>> UpdateProfile([FromBody] UserProfile updatedProfile)
    {
        var userId = HttpContext.User.GetUserId();

        if (string.IsNullOrEmpty(userId))
            return Unauthorized("User ID not found in token.");

        var profile = await _profileService.GetOrCreateAsync(userId);

        // Only update fields that are provided
        if (updatedProfile.Age != null) profile.Age = updatedProfile.Age;
        if (updatedProfile.WeightKg != null) profile.WeightKg = updatedProfile.WeightKg;
        if (updatedProfile.HeightCm != null) profile.HeightCm = updatedProfile.HeightCm;
        if (updatedProfile.Gender != null) profile.Gender = updatedProfile.Gender;
        if (updatedProfile.FitnessLevel != null) profile.FitnessLevel = updatedProfile.FitnessLevel;
        if (updatedProfile.BodyFatPercentage != null) profile.BodyFatPercentage = updatedProfile.BodyFatPercentage;

        await _profileService.SaveAsync(profile);
        return Ok(profile);
    }
}