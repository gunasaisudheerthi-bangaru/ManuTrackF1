using System.Diagnostics;

namespace ManuTrack.SharedKernel.Exceptions;

[DebuggerDisplay("{Message}")]
public class NotFoundException : Exception
{
    public NotFoundException(string resource, object id)
        : base($"{resource} with ID '{id}' was not found.") { }
    public NotFoundException(string message) : base(message) { }
}

[DebuggerDisplay("{Message}")]
public class ValidationException : Exception
{
    public List<string> Errors { get; }
    public ValidationException(string error) : base(error) => Errors = [error];
    public ValidationException(List<string> errors) : base("Validation failed.") => Errors = errors;
}

[DebuggerDisplay("{Message}")]
public class ForbiddenException : Exception
{
    public ForbiddenException(string message = "You do not have permission.") : base(message) { }
}

[DebuggerDisplay("{Message}")]
public class UnauthorizedException : Exception
{
    public UnauthorizedException(string message = "Authentication failed.") : base(message) { }
}

[DebuggerDisplay("{Message}")]
public class ConflictException : Exception
{
    public ConflictException(string message) : base(message) { }
}
