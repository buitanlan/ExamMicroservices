using Microsoft.IdentityModel.Logging;
using PortalApp.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor();

builder.Services.AddRazorPages();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = AuthenticationConsts.SignInScheme;
        options.DefaultChallengeScheme = AuthenticationConsts.OidcAuthenticationScheme;
    })
    .AddCookie(AuthenticationConsts.SignInScheme, options =>
    {
        options.Cookie.Name = builder.Configuration.GetValue<string>("IdentityServerConfig:CookieName");
        options.LoginPath = "/login.html";
    })
    .AddOpenIdConnect(AuthenticationConsts.OidcAuthenticationScheme, options =>
    {
        options.Authority = builder.Configuration.GetValue<string>("IdentityServerConfig:IdentityServerUrl");
        options.ClientId = builder.Configuration.GetValue<string>("IdentityServerConfig:ClientId");
        options.ClientSecret = builder.Configuration.GetValue<string>("IdentityServerConfig:ClientSecret");
        options.ResponseType = "code";
        options.RequireHttpsMetadata = false;
        options.SaveTokens = true;
    });
var app = builder.Build();

// Configure the HTTP request pipeline.
IdentityModelEventSource.ShowPII = true;

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
