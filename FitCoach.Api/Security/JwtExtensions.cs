using System.Security.Claims;

namespace FitCoach.Api.Security;

// Helper methods to extract user info from JWT claims.
// Usage in any controller: var userId = User.GetUserId();
public static class JwtExtensions
{
    // Extracts the user ID from the JWT token
    public static string GetUserId(this ClaimsPrincipal user)
        => user.FindFirst(ClaimTypes.NameIdentifier)?.Value
           ?? user.FindFirst("sub")?.Value
           ?? string.Empty;

    // Extracts the subscription status (free, premium, etc.)
    public static string GetSubscriptionStatus(this ClaimsPrincipal user)
        => user.FindFirst("statutAbonnement")?.Value ?? "free";
    
    public static string GetUserName(this ClaimsPrincipal user)
    {
        return user.FindFirstValue(ClaimTypes.Name)
               ?? user.FindFirstValue("name")
               ?? "User";
    }
}