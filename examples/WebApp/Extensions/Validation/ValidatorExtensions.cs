using FluentValidation;
using FunctionalPrimitives;
using FunctionalPrimitives.Monads.Results;

namespace WebApp.Extensions.Validation;

public static class ValidatorExtensions
{
    public static async Task<Result<T>> ValidateToResultAsync<T>(
        this IValidator<T> validator,
        T instance)
    {
        var result = await validator.ValidateAsync(instance);

        return result.IsValid
            ? instance
            : Failure<T>(result.ToErrors());
    }
}
