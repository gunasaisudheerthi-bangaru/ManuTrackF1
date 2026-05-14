using ManuTrack.SharedKernel.Exceptions;
using ManuTrack.SharedKernel.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace ManuTrack.SharedKernel.Filters;

public class GlobalExceptionFilter : IExceptionFilter
{
    private readonly ILogger<GlobalExceptionFilter> _logger;

    public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
    {
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        _logger.LogError(context.Exception,
            "Unhandled exception on {Method} {Path}: {Message}",
            context.HttpContext.Request.Method,
            context.HttpContext.Request.Path,
            context.Exception.Message);

        var (statusCode, response) = context.Exception switch
        {
            NotFoundException ex => (404, ApiResponse.Fail(ex.Message)),
            ValidationException ex => (400, ApiResponse.Fail(ex.Errors)),
            UnauthorizedException ex => (401, ApiResponse.Fail(ex.Message)),
            ForbiddenException ex => (403, ApiResponse.Fail(ex.Message)),
            ConflictException ex => (409, ApiResponse.Fail(ex.Message)),
            _ => (500, ApiResponse.Fail("An unexpected error occurred."))
        };

        context.Result = new ObjectResult(response) { StatusCode = statusCode };
        context.ExceptionHandled = true;
    }
}