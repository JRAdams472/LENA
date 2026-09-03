using FluentValidation;

using LENA.Application.Exceptions;

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LENA.API.ExceptionHandling
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            var problemDetails = new ProblemDetails
            {
                Instance = httpContext.Request.Path,
            };

            if (exception is ValidationException validationException)
            {
                problemDetails.Status = StatusCodes.Status400BadRequest;
                problemDetails.Title = "Validation failed";
                problemDetails.Detail = string.Join("; ", validationException.Errors.Select(e => e.ErrorMessage));
            }
            else if (exception is NotFoundException notFoundException)
            {
                problemDetails.Status = StatusCodes.Status404NotFound;
                problemDetails.Title = "Resource not found";
                problemDetails.Detail = notFoundException.Message;
            }
            else if (exception is LENA.Application.Exceptions.UnauthenticatedUserException)
            {
                problemDetails.Status = StatusCodes.Status401Unauthorized;
                problemDetails.Title = "Unauthenticated";
                problemDetails.Detail = exception.Message;
            }
            else if (exception is ArgumentException)
            {
                problemDetails.Status = StatusCodes.Status400BadRequest;
                problemDetails.Title = "Bad request";
                problemDetails.Detail = exception.Message;
            }
            else if ((exception is OperationCanceledException || exception is IOException) && httpContext.RequestAborted.IsCancellationRequested)
            {
                problemDetails.Status = StatusCodes.Status499ClientClosedRequest;
                problemDetails.Title = "Request canceled";
                problemDetails.Detail = "The client closed the request before a response could be sent.";
            }
            else
            {
                _logger.LogError(exception, "Unhandled exception at {Path}", httpContext.Request.Path);
                problemDetails.Status = StatusCodes.Status500InternalServerError;
                problemDetails.Title = "An unexpected error occurred";
                problemDetails.Detail = "An internal error occurred. Please try again later.";
            }

            httpContext.Response.StatusCode = problemDetails.Status.Value;
            httpContext.Response.ContentType = "application/problem+json";

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
            return true;
        }
    }
}