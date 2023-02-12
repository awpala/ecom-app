using System.Net;
using System.Text.Json;
using API.Errors;

namespace API.Middleware
{
  public class ExceptionMiddleware
  {
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
    {
      _next = next;
      _logger = logger;
      _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
      try
      {
        // pass onto next stage, if no exception(s) occurs
        await _next(context);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, ex.Message);
        context.Response.ContentType = "application/json";
        var errorCode = (int)HttpStatusCode.InternalServerError;
        context.Response.StatusCode = errorCode;

        var response = _env.IsDevelopment()
          ? new ApiException(errorCode, ex.Message, ex.StackTrace.ToString())
          : new ApiException(errorCode);

        var options = new JsonSerializerOptions{ PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        var json = JsonSerializer.Serialize(response, options);
        await context.Response.WriteAsync(json);
      }
    }
  }
}
