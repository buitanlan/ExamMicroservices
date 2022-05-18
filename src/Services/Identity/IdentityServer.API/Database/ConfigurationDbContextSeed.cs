using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Entities;
using Duende.IdentityServer.EntityFramework.Mappers;
using IdentityServer.API.Configuration;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.API.Database;

public static class  ConfigurationDbContextSeed
{
    public static async Task SeedAsync(ConfigurationDbContext context, IConfiguration configuration)
    {

        //callbacks urls from config:
        var clientUrls = new Dictionary<string, string>
        {
            {"ExamWebApp", configuration.GetValue<string>("ExamWebAppClient")},
            {"ExamWebAdmin", configuration.GetValue<string>("ExamWebAdminClient")},
            {"ExamApi", configuration.GetValue<string>("ExamWebApiClient")}
        };


        if (!await context.Clients.AnyAsync())
        {
            foreach (var client in Config.GetClients(clientUrls))
            {
                var entity = client.ToEntity();
                context.Clients.Add(entity);
            }
        }
        else
        {
            List<ClientRedirectUri> oldRedirects = await context.Clients.Include(c => c.RedirectUris)
                .SelectMany(c => c.RedirectUris)
                .Where(ru => ru.RedirectUri.EndsWith("/o2c.html"))
                .ToListAsync();

            if (oldRedirects.Any())
            {
                foreach (var ru in oldRedirects)
                {
                    ru.RedirectUri = ru.RedirectUri.Replace("/o2c.html", "/oauth2-redirect.html");
                    context.Update(ru.Client);
                }
            }
        }

        if (!await context.IdentityResources.AnyAsync())
        {
            foreach (var resource in Config.GetIdentityResources())
            {
                context.IdentityResources.Add(resource.ToEntity());
            }
        }

        if (!await context.ApiResources.AnyAsync())
        {
            foreach (var api in Config.GetApis())
            {
                context.ApiResources.Add(api.ToEntity());
            }

        }

        if (!await context.ApiScopes.AnyAsync())
        {
            foreach (var resource in Config.GetApiScopes())
            {
                context.ApiScopes.Add(resource.ToEntity());
            }
        }

        await context.SaveChangesAsync();
    }
}