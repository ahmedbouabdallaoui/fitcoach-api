using FitCoach.Api.DTOs.Requests;
using FitCoach.Api.DTOs.Responses;
using FitCoach.Api.Security;
using FitCoach.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitCoach.Api.Controllers;

[ApiController]
[Route("api/chat")]
[Authorize]
public class ChatController : ControllerBase
{
    private readonly IChatService _chatService;
    private readonly ILogger<ChatController> _logger;

    public ChatController(
        IChatService chatService,
        ILogger<ChatController> logger)
    {
        _chatService = chatService;
        _logger = logger;
    }

    /// <summary>
    /// Send a message to the FitCoach AI assistant.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ChatResponse>> SendMessage([FromBody] ChatRequest request)
    {
        var userId = HttpContext.User.GetUserId();
        var userName = HttpContext.User.GetUserName();

        if (string.IsNullOrEmpty(userId))
            return Unauthorized("User ID not found in token.");

        _logger.LogInformation(
            "Message received from user {UserId} tag {Tag}",
            userId, request.Tag ?? "none"
        );

        var response = await _chatService.ProcessMessageAsync(request, userId, userName);
        return Ok(response);
    }
}