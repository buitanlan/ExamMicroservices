using System.Threading.Tasks;
using Duende.IdentityServer.Validation;
using IdentityModel;
using IdentityServer.Admin.EntityFramework.Shared.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Serilog;

namespace IdentityServer.STS.Identity.Services;

public class CustomResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
{
    private readonly SignInManager<UserIdentity> _signInManager;
    private readonly UserManager<UserIdentity> _userManager;
    private readonly ILogger<CustomResourceOwnerPasswordValidator> _logger;

    public CustomResourceOwnerPasswordValidator(
        SignInManager<UserIdentity> signInManager,
        UserManager<UserIdentity> userManager,
        ILogger<CustomResourceOwnerPasswordValidator> logger)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _logger = logger;
    }

    public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
    {
        var signInResult = await _signInManager.PasswordSignInAsync(context.UserName, context.Password, false, false);
        if (signInResult.Succeeded)
        {
            Log.Fatal("Login from Resource Owner Password is success");

            var user = await _userManager.FindByNameAsync(context.UserName);
            context.Result = new GrantValidationResult(user.Id, OidcConstants.AuthenticationMethods.Password);
        }
        Log.Fatal("Login from Resource Owner Password is failed");
    }
}