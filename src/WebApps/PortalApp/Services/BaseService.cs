using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Examination.Shared.SeedWork;
using Microsoft.AspNetCore.Authentication;

namespace PortalApp.Services;

public class BaseService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
{
    private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };

    public async Task<ApiResult<T>> GetAsync<T>(string url, bool isSecuredServie = false)
    {
        using var client = httpClientFactory.CreateClient("BackendApi");
        if (isSecuredServie)
        {
            var token = await httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        return await client.GetFromJsonAsync<ApiResult<T>>(url, _options);
    }

    public async Task<ApiResult<TResponse>> PostAsync<TRequest, TResponse>(string url, TRequest requestContent,
        bool isSecuredServie = true)
    {
        var client = httpClientFactory.CreateClient("BackendApi");
        StringContent httpContent = null;
        if (requestContent != null)
        {
            var json = JsonSerializer.Serialize(requestContent);
            httpContent = new StringContent(json, Encoding.UTF8, "application/json");
        }

        if (isSecuredServie)
        {
            var token = await httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        var response = await client.PostAsync(url, httpContent);
        var body = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            return JsonSerializer.Deserialize<ApiResult<TResponse>>(body, _options);
        }

        throw new Exception(body);
    }

    public async Task<ApiResult<bool>> PutAsync<TRequest, TResponse>(string url, TRequest requestContent,
        bool isSecuredServie = true)
    {
        var client = httpClientFactory.CreateClient("BackendApi");
        HttpContent httpContent = null;
        if (requestContent != null)
        {
            var json = JsonSerializer.Serialize(requestContent);
            httpContent = new StringContent(json, Encoding.UTF8, "application/json");
        }

        if (isSecuredServie)
        {
            var token = await httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        var response = await client.PutAsJsonAsync(url, httpContent);
        var body = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<ApiResult<bool>>(body, _options);
    }
}
