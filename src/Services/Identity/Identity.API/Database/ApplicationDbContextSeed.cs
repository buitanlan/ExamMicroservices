using Identity.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Identity.API.Database;

public static class ApplicationDbContextSeed
{

    public static async Task SeedAsync(ApplicationDbContext context, int? retry = 0)
    {
        if (retry != null)
        {
            int retryForAvailability = retry.Value;

            try
            {

                if (!await context.Users.AnyAsync())
                {
                    context.Users.AddRange(GetDefaultUser());

                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                if (retryForAvailability < 10)
                {
                    retryForAvailability++;

                    Log.Error(ex, "EXCEPTION ERROR while migrating {DbContextName}", nameof(ApplicationDbContext));

                    await SeedAsync(context, retryForAvailability);
                }
            }
        }
    }
    private static IEnumerable<ApplicationUser> GetDefaultUser()
    {
        var user = new ApplicationUser
            {
                Email = "admin@demo.com",
                Id = Guid.NewGuid().ToString(),
                LastName = "Account",
                FirstName = "Demo",
                PhoneNumber = "1234567890",
                UserName = "admin@demo.com",
                NormalizedEmail = "ADMIN@DEMO.COM",
                NormalizedUserName = "ADMIN@DEMO.COM",
                SecurityStamp = Guid.NewGuid().ToString("D"),
            };

        user.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(user, "Admin@123$");

        return new List<ApplicationUser>
        {
            user
        };
    }
}