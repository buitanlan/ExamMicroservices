using Microsoft.EntityFrameworkCore;
using Npgsql;
using Polly;
using Serilog;

namespace Identity.API.Extensions;

public static class HostExtensions
{
    public static IHost MigrateDbContext<TContext>(this IHost host, Action<TContext, IServiceProvider> seeder) where TContext : DbContext
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;
        var context = services.GetService<TContext>();

        try
        {
            Log.Information("Migrating database associated with context {DbContextName}", typeof(TContext).Name);

            var retries = 10;
            var retry = Policy.Handle<NpgsqlException>()
                .WaitAndRetry(
                    retryCount: retries,
                    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    onRetry: (exception, _, retry, _) =>
                    {
                        Log.Warning(exception, "[{Prefix}] Exception {ExceptionType} with message {Message} detected on attempt {Retry} of {Retries}", nameof(TContext), exception.GetType().Name, exception.Message, retry, retries);
                    });

            retry.Execute(() => InvokeSeeder(seeder, context, services));

            Log.Information("Migrated database associated with context {DbContextName}", typeof(TContext).Name);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while migrating the database used on context {DbContextName}", typeof(TContext).Name);
        }

        return host;
    }

    private static async Task InvokeSeeder<TContext>(Action<TContext, IServiceProvider> seeder, TContext context, IServiceProvider services)
        where TContext : DbContext
    {
        await context.Database.MigrateAsync();
        seeder(context, services);
    }
}