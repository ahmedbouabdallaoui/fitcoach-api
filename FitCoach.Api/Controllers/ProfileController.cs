using FitCoach.Api.Domain.Entities;
using FitCoach.Api.DTOs.Requests;
using FitCoach.Api.Security;
using FitCoach.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitCoach.Api.Controllers;

[ApiController]
[Route("api/profile")] public class ProfileController : ControllerBase
{
    private readonly IProfileService _profileService;
    private readonly ILogger<ProfileController> _logger;
    String userId = "test-user-123";

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
        var userId = "test-user-123";

        var profile = await _profileService.GetOrCreateAsync(userId);
        return Ok(profile);
    }
    
    [HttpPut]
    public async Task<ActionResult<UserProfile>> UpdateProfile([FromBody] UpdateProfileRequest request)
    {
        var userId = "test-user-123";

        var profile = await _profileService.GetOrCreateAsync(userId);

        if (request.Age != null) profile.Age = request.Age;
        if (request.WeightKg != null) profile.WeightKg = request.WeightKg;
        if (request.HeightCm != null) profile.HeightCm = request.HeightCm;
        if (request.Gender != null) profile.Gender = request.Gender;
        if (request.FitnessLevel != null) profile.FitnessLevel = request.FitnessLevel;
        if (request.BodyFatPercentage != null) profile.BodyFatPercentage = request.BodyFatPercentage;

        await _profileService.SaveAsync(profile);
        return Ok(profile);
    }
}