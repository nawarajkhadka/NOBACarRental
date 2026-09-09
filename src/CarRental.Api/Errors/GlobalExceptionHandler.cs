using Microsoft.AspNetCore.Diagnostics;

namespace CarRental.Api.Errors;

/// <summary>
/// Catches any exception that escapes a controller action, maps it to the API's
/// error contract via <see cref="IExceptionErrorCodeMapper"/>, and writes it as the response.
/// </summary>
public sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly IExceptionErrorCodeMapper _mapper;
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(IExceptionErrorCodeMapper mapper, ILogger<GlobalExceptionHandler> logger)
    {
        _mapper = mapper;
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var error = _mapper.Map(exception);

        if (error.ErrorCode == ErrorCode.Unknown)
        {
            _logger.LogError(exception, "Unhandled exception occurred while processing {Method} {Path}.",
                httpContext.Request.Method, httpContext.Request.Path);
        }

        httpContext.Response.StatusCode = (int)error.StatusCode;
        await httpContext.Response.WriteAsJsonAsync(
            new ApiErrorResponse(error.ErrorCode, error.Message),
            cancellationToken);

        return true;
    }
}
