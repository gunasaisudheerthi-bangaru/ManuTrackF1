using ManuTrack.SharedKernel.Exceptions;
using ManuTrack.SharedKernel.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ManuTrack.SharedKernel.Filters;

/// <summary>
/// Wraps every controller action in a try-catch at the USER CODE level.
/// VS debugger will NOT break because exceptions are caught before leaving user code.
/// </summary>
public class GlobalExceptionFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        try
        {
            var executed = await next();

            // If the action itself set an exception (unlikely but safe)
            if (executed.Exception != null && !executed.ExceptionHandled)
            {
                executed.Result = MapException(executed.Exception);
                executed.ExceptionHandled = true;
            }
        }
        catch (Exception ex)
        {
            context.Result = MapException(ex);
        }
    }

    private static IActionResult MapException(Exception ex) => ex switch
    {
        NotFoundException e      => new NotFoundObjectResult(ApiResponse.Fail(e.Message)),
        ValidationException e    => new BadRequestObjectResult(ApiResponse.Fail(e.Errors)),
        UnauthorizedException e  => new UnauthorizedObjectResult(ApiResponse.Fail(e.Message)),
        ConflictException e      => new ConflictObjectResult(ApiResponse.Fail(e.Message)),
        ForbiddenException e     => new ObjectResult(ApiResponse.Fail(e.Message)) { StatusCode = 403 },
        _                        => new BadRequestObjectResult(ApiResponse.Fail(ex.Message))
    };
}
