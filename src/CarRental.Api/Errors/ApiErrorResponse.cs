namespace CarRental.Api.Errors;

public sealed record ApiErrorResponse(ErrorCode ErrorCode, string Message);
