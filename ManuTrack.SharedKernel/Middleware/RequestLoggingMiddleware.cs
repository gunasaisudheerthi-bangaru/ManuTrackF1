using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace ManuTrack.SharedKernel.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        var requestId = Guid.NewGuid().ToString()[..8].ToUpper();

        _logger.LogInformation("[{RequestId}] → {Method} {Path}",
            requestId, context.Request.Method, context.Request.Path);

        await _next(context);

        stopwatch.Stop();

        _logger.LogInformation("[{RequestId}] ← {StatusCode} ({ElapsedMs}ms)",
            requestId, context.Response.StatusCode, stopwatch.ElapsedMilliseconds);
    }
}