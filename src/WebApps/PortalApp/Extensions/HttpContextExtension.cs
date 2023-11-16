using System.Security.Claims;
using IdentityModel;

namespace PortalApp.Extensions;

public static class HttpContextExtensions
{
    public static string GetUserId(this ClaimsPrincipal claimsPrincipal)
    {
        var claim = ((ClaimsIdentity)claimsPrincipal.Identity)
            .Claims
            .SingleOrDefault(x => x.Type == JwtClaimTypes.Subject);
        return claim?.Value;
    }

    public static string GetEmail(this ClaimsPrincipal claimsPrincipal)
    {
        var claim = ((ClaimsIdentity)claimsPrincipal.Identity)
            .Claims
            .SingleOrDefault(x => x.Type == ClaimTypes.Email);
        return claim?.Value;
    }
}
