using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NovaStore.Application.Common.Exceptions;

namespace NovaStore.Api.ExceptionHandling;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
  private readonly ILogger<GlobalExceptionHandler> _logger;

  public GlobalExceptionHandler(
      ILogger<GlobalExceptionHandler> logger)
  {
    _logger = logger;
  }

  public async ValueTask<bool> TryHandleAsync(
      HttpContext httpContext,
      Exception exception,
      CancellationToken cancellationToken)
  {
    ProblemDetails problemDetails;

    switch (exception)
    {
      case ValidationException validationException:
        problemDetails = new ProblemDetails
        {
          Status = StatusCodes.Status400BadRequest,
          Title = "Validation Error",
          Detail = validationException.Message
        };

        problemDetails.Extensions["errors"] =
            validationException.Errors;

        break;

      case ConflictException:
        problemDetails = new ProblemDetails
        {
          Status = StatusCodes.Status409Conflict,
          Title = "Conflict",
          Detail = exception.Message
        };

        break;

      default:
        problemDetails = new ProblemDetails
        {
          Status = StatusCodes.Status500InternalServerError,
          Title = "Internal Server Error",
          Detail = "An unexpected error occurred."
        };

        _logger.LogError(
            exception,
            "An unhandled exception occurred.");

        break;
    }

    httpContext.Response.StatusCode = problemDetails.Status!.Value;

    await httpContext.Response.WriteAsJsonAsync(
        problemDetails,
        cancellationToken);

    return true;
  }
}