using System.Linq;
using System.Net;
using CarRental.Application.Exceptions;
using CarRental.Domain.Exceptions;
using FluentValidation;

namespace CarRental.Api.Errors;

public interface IExceptionErrorCodeMapper
{
    ApiError Map(Exception exception);
}

public sealed record ApiError(HttpStatusCode StatusCode, ErrorCode ErrorCode, string Message);

/// <summary>
/// Translates internal exceptions raised by the Application/Domain layers into the
/// API's public error contract (HTTP status + business error code + safe message).
/// </summary>
public sealed class ExceptionErrorCodeMapper : IExceptionErrorCodeMapper
{
    public ApiError Map(Exception exception) => exception switch
    {
        // Request-shape validation (FluentValidation), always populated with exactly one failure.
        ValidationException { Errors: var errors } => new ApiError(
            HttpStatusCode.BadRequest, ErrorCode.ValidationError, errors.First().ErrorMessage),

        EntityNotFoundException => new ApiError(HttpStatusCode.NotFound, ErrorCode.NotFound, exception.Message),
        ApplicationValidationException => new ApiError(HttpStatusCode.Conflict, ErrorCode.Conflict, exception.Message),
        DomainValidationException => new ApiError(HttpStatusCode.BadRequest, ErrorCode.ValidationError, exception.Message),

        // Unrecognized exceptions may carry internal details (stack traces, connection
        // strings, EF error text) — never surface exception.Message for these.
        _ => new ApiError(HttpStatusCode.InternalServerError, ErrorCode.Unknown, "An unexpected error occurred.")
    };
}
