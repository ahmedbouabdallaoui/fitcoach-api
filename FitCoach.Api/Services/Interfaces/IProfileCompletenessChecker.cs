using FitCoach.Api.Domain.Entities;

namespace FitCoach.Api.Services.Interfaces;

public interface IProfileCompletenessChecker
{
    List<string> GetMissingProfileFields(UserProfile profile);
    bool IsProfileComplete(UserProfile profile);
    List<string> GetMissingContextFields(ConversationContext context, string tag);
    bool IsContextReady(ConversationContext context, string tag);
}