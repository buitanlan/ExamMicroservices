using System.Text.Json;
using Examination.Shared.SeedWork;

namespace Examination.API.Middlewares;

public class ErrorWrappingMiddleware(RequestDelegate next, ILogger<ErrorWrappingMiddleware> logger)
{

    public async Task Invoke(HttpContext context)
    {
        var errorMsg = string.Empty;
        try
        {
            await next.Invoke(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            errorMsg = ex.Message;
            context.Response.StatusCode = 500;
        }

        if (!context.Response.HasStarted && context.Response.StatusCode != 204)
        {
            context.Response.ContentType = "application/json";

            var response = new ApiErrorResult<bool>(errorMsg);

            var json = JsonSerializer.Serialize(response);

            await context.Response.WriteAsync(json);
        }
    }
}
