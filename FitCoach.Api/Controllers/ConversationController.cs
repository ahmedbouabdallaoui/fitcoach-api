using FitCoach.Api.DTOs.Responses;
using FitCoach.Api.Mappers;
using FitCoach.Api.Security;
using FitCoach.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitCoach.Api.Controllers;

[ApiController]
[Route("api/conversations")]
[Authorize]
public class ConversationController : ControllerBase
{
    private readonly IConversationService _conversationService;
    private readonly ILogger<ConversationController> _logger;

    public ConversationController(
        IConversationService conversationService,
        ILogger<ConversationController> logger)
    {
        _conversationService = conversationService;
        _logger = logger;
    }
    
    [HttpGet]
    public async Task<ActionResult<List<ConversationResponse>>> GetConversations()
    {
        var userId = HttpContext.User.GetUserId();

        if (string.IsNullOrEmpty(userId))
            return Unauthorized("User ID not found in token.");

        var conversations = await _conversationService.GetUserConversationsAsync(userId);
        return Ok(conversations.Select(ConversationMapper.ToResponse));
    }
    
    [HttpGet("{conversationId}")]
    public async Task<ActionResult<ConversationResponse>> GetConversation(string conversationId)
    {
        var userId = HttpContext.User.GetUserId();

        if (string.IsNullOrEmpty(userId))
            return Unauthorized("User ID not found in token.");

        var conversation = await _conversationService.GetConversationAsync(conversationId, userId);

        if (conversation == null)
            return NotFound("Conversation not found.");

        return Ok(ConversationMapper.ToResponse(conversation));
    }
    
    [HttpDelete("{conversationId}")]
    public async Task<IActionResult> DeleteConversation(string conversationId)
    {
        var userId = HttpContext.User.GetUserId();

        if (string.IsNullOrEmpty(userId))
            return Unauthorized("User ID not found in token.");

        await _conversationService.DeleteConversationAsync(conversationId, userId);
        return NoContent();
    }
}