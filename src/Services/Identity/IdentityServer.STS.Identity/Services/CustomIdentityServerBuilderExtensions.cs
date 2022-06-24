using IdentityServer.Admin.Services;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityServer.STS.Identity.Services;

public static class CustomIdentityServerBuilderExtensions
{
    public static IIdentityServerBuilder AddCustomUserStore(this IIdentityServerBuilder builder)
    {
        builder.AddProfileService<CustomProfileService>();
        builder.AddResourceOwnerValidator<CustomResourceOwnerPasswordValidator>();

        return builder;
    }
}