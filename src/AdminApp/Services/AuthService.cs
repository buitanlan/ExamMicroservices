using AdminApp.Core;
using AdminApp.Core.Authentication;
using AdminApp.Models;
using AdminApp.Services.Interfaces;
using Blazored.SessionStorage;
using IdentityModel.Client;
using Microsoft.AspNetCore.Components.Authorization;
using Serilog;

namespace AdminApp.Services;

public class AuthService(HttpClient httpClient,
        ILogger<AuthService> logger,
        ISessionStorageService localStorage,
        AuthenticationStateProvider authenticationStateProvider,
        IConfiguration configuration)
    : IAuthService
{
    private static DiscoveryDocumentResponse _disco;
    private readonly ILogger<AuthService> _logger = logger;

    public async Task<TokenResponse> LoginAsync(LoginRequest loginRequest)
    {
        _disco = await HttpClientDiscoveryExtensions.GetDiscoveryDocumentAsync(
            httpClient,
            configuration["IdentityServerConfig:IdentityServerUrl"]);

        if (_disco.IsError)
        {
            throw new ApplicationException($"Status code: {_disco.IsError}, Error: {_disco.Error}");
        }

        var token = await RequestTokenAsync(loginRequest.UserName, loginRequest.Password);
        if (token.IsError == false)
        {
            await localStorage.SetItemAsync(KeyConstants.AccessToken, token.AccessToken);
            await localStorage.SetItemAsync(KeyConstants.RefreshToken, token.RefreshToken);
            ((ApiAuthenticationStateProvider)authenticationStateProvider).MarkUserAsAuthenticated(loginRequest.UserName);

        }
        return token;
    }


    private async Task<TokenResponse> RequestTokenAsync(string user, string password)
    {
        Log.Information("begin RequestTokenAsync");
        var response = await httpClient.RequestPasswordTokenAsync(new PasswordTokenRequest
        {
            Address = _disco.TokenEndpoint,

            ClientId = configuration["IdentityServerConfig:ClientId"],
            ClientSecret = configuration["IdentityServerConfig:ClientSecret"],
            Scope = "email openid roles profile offline_access",

            UserName = user,
            Password = password
        });

        return response;
    }

    public async Task LogoutAsync()
    {
        await localStorage.RemoveItemAsync(KeyConstants.AccessToken);
        await localStorage.RemoveItemAsync(KeyConstants.RefreshToken);
        ((ApiAuthenticationStateProvider)authenticationStateProvider).MarkUserAsLoggedOut();
        httpClient.DefaultRequestHeaders.Authorization = null;
    }
}