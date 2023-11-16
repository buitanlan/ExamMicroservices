using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityServer.Admin.EntityFramework.Shared.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace IdentityServer.Admin.Services;

public class CustomProfileService : IProfileService
{
    private readonly UserManager<UserIdentity> _userManager;
    private readonly ILogger<CustomProfileService> _logger;

    public CustomProfileService(
        UserManager<UserIdentity> userManager,
        ILogger<CustomProfileService> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        _logger.LogDebug("Get profile called for subject {subject} from client {client} with claim types {claimTypes} via {caller}",
            context.Subject.GetSubjectId(),
            context.Client.ClientName ?? context.Client.ClientId,
            context.RequestedClaimTypes,
            context.Caller);

        var user = await _userManager.FindByIdAsync(context.Subject.GetSubjectId());
        var claimsFromDb = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);
        var claims = new List<Claim>
        {
            new(ClaimTypes.Role, string.Join(";",roles)),
            new(ClaimTypes.Email, user.Email),
        };
        claims.AddRange(claimsFromDb);

        context.IssuedClaims = claims;
    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
        var sub = context.Subject.GetSubjectId();
        var user = await _userManager.FindByIdAsync(sub);
        context.IsActive = user != null;
    }
}
