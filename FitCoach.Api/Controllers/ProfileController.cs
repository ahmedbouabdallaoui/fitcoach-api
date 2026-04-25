using FitCoach.Api.Domain.Entities;
using FitCoach.Api.DTOs.Requests;
using FitCoach.Api.Security;
using FitCoach.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitCoach.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/profile")]
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
        var userId = HttpContext.GetUserId();
        if (string.IsNullOrWhiteSpace(userId))
            return Unauthorized("Missing user identity.");

        var userName = HttpContext.User.GetUserName();
        var profile = await _profileService.GetOrCreateAsync(userId, userName);
        return Ok(profile);
    }
    
    [HttpPut]
    public async Task<ActionResult<UserProfile>> UpdateProfile([FromBody] UpdateProfileRequest request)
    {
        var userId = HttpContext.GetUserId();
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized("Missing user identity.");
        }

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
