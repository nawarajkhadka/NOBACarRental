using FluentValidation;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CarRental.Api.Validation;

/// <summary>
/// Runs FluentValidation against any action argument for which an IValidator{T} is
/// registered, before the action executes. Stops and throws on the first validation
/// failure found — request-shape validation is meant to fail fast, before any
/// business/data validation in the service layer runs.
/// </summary>
public sealed class ValidationActionFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        foreach (var argument in context.ActionArguments.Values)
        {
            if (argument is null)
            {
                continue;
            }

            var validatorType = typeof(IValidator<>).MakeGenericType(argument.GetType());
            if (context.HttpContext.RequestServices.GetService(validatorType) is not IValidator validator)
            {
                continue;
            }

            var result = await validator.ValidateAsync(new ValidationContext<object>(argument));
            if (!result.IsValid)
            {
                throw new ValidationException(new[] { result.Errors[0] });
            }
        }

        await next();
    }
}
