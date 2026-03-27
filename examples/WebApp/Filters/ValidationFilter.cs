using FluentValidation;
using WebApp.Extensions.Http;
using WebApp.Extensions.Validation;

namespace WebApp.Filters;

public class ValidationFilter<T>(IValidator<T>? validator) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        if (validator is null)
            return await next(context);

        var argument = context.Arguments
            .FirstOrDefault(x => x?.GetType() == typeof(T));

        if (argument is null)
            return await next(context);

        var validationResult = await validator.ValidateToResultAsync((T)argument);

        if (validationResult.IsSuccess)
            return await next(context);

        return validationResult.ToHttpResult();
    }
}


