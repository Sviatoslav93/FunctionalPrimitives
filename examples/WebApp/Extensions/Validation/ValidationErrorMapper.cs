using FunctionalPrimitives.Errors;
using FunctionalPrimitives.Errors.Extensions;

namespace WebApp.Extensions.Validation;

public static class ValidationErrorMapper
{
    public static Error[] ToErrors(this FluentValidation.Results.ValidationResult result)
    {
        return result.Errors.Select(e =>
            new ValidationError(
                e.ErrorCode,
                e.ErrorMessage)
                .WithMetadata("property", e.PropertyName)).ToArray();
    }
}

