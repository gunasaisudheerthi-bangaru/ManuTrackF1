using System.Text.Json;
using ManuTrack.SharedKernel.Exceptions;
using ManuTrack.SharedKernel.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ManuTrack.SharedKernel.Middleware;

/// <summary>
/// Catches ALL exceptions at the middleware level (before VS debugger sees them as unhandled).
/// Converts them to clean JSON responses — no exceptions propagate to the debugger.
/// </summary>
public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception on {Method} {Path}: {Message}",
                context.Request.Method, context.Request.Path, ex.Message);

            await WriteErrorResponse(context, ex);
        }
    }

    private static async Task WriteErrorResponse(HttpContext context, Exception ex)
    {
        var (statusCode, message, errors) = ex switch
        {
            NotFoundException nfe    => (404, nfe.Message,  new List<string> { nfe.Message }),
            ValidationException ve   => (400, ve.Message,   ve.Errors),
            UnauthorizedException ue => (401, ue.Message,   new List<string> { ue.Message }),
            ForbiddenException fe    => (403, fe.Message,   new List<string> { fe.Message }),
            ConflictException ce     => (409, ce.Message,   new List<string> { ce.Message }),
            _                        => (500, "An unexpected error occurred.", new List<string> { "An unexpected error occurred." })
        };

        var response = new ApiResponse
        {
            Success = false,
            Message = message,
            Errors  = errors
        };

        context.Response.StatusCode  = statusCode;
        context.Response.ContentType = "application/json";

        var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(json);
    }
}
