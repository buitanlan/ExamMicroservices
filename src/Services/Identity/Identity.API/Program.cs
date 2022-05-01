using System.Reflection;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.Services;
using Identity.API;
using Identity.API.Database;
using Identity.API.Models;
using Identity.API.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder();



var migrationsAssembly = typeof(Program).GetTypeInfo().Assembly.GetName().Name;
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddControllers();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.Configure<AppSettings>(builder.Configuration);

builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console()
    .ReadFrom.Configuration(ctx.Configuration));

builder.Services.AddDbContextPool<ApplicationDbContext>(options =>
       options.UseNpgsql(connectionString,
       npgsqlOptionsAction: sqlOptions =>
       {
           sqlOptions.MigrationsAssembly(migrationsAssembly);
           sqlOptions.EnableRetryOnFailure(
               maxRetryCount: 5,
               maxRetryDelay: TimeSpan.FromSeconds(30),
               errorCodesToAdd: null);
       }));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddIdentityServer(x =>
    {
        x.IssuerUri = "https://tedu.com.vn";
        x.Authentication.CookieLifetime = TimeSpan.FromHours(2);
    })
    .AddDeveloperSigningCredential()
    .AddAspNetIdentity<ApplicationUser>()
    .AddConfigurationStore(options =>
    {
        options.ConfigureDbContext = optionsBuilder => optionsBuilder.UseNpgsql(connectionString,
            npgsqlOptionsAction: sqlOptions =>
            {
                sqlOptions.MigrationsAssembly(migrationsAssembly);
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorCodesToAdd: null);
            });
    })
    .AddOperationalStore(options =>
    {
        options.ConfigureDbContext = optionsBuilder => optionsBuilder.UseNpgsql(connectionString,
            npgsqlOptionsAction: sqlOptions =>
            {
                sqlOptions.MigrationsAssembly(migrationsAssembly);
                sqlOptions.EnableRetryOnFailure(maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorCodesToAdd: null);
            });
    });
builder.Services.AddTransient<IProfileService, ProfileService>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Identity.API", Version = "v1" });
});


var app = builder.Build();
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try
{
    Log.Information("Configuring web host");
    Log.Information("Applying migrations");
    var applicationDbContext = services.GetRequiredService<ApplicationDbContext>();
    await applicationDbContext.Database.MigrateAsync();
    await ApplicationDbContextSeed.SeedAsync(applicationDbContext, 10);
    var configurationDbContext = services.GetRequiredService<ConfigurationDbContext>();
    await configurationDbContext.Database.MigrateAsync();
    await ConfigurationDbContextSeed.SeedAsync(configurationDbContext, app.Configuration);
    
    var persistedGrantDbContext = services.GetRequiredService<PersistedGrantDbContext>();
    await persistedGrantDbContext.Database.MigrateAsync();
    Log.Information("Starting web host ");

}
catch (Exception ex)
{
    Log.Fatal(ex, "Program terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Identity.API v1"));
}
app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseRouting();

app.UseIdentityServer();

app.UseAuthorization();


app.MapGet("/", () => "Hello World!");

app.Run();