using FunctionalPrimitives;
using FunctionalPrimitives.Extensions.Errors;
using WebApp.Shared;

namespace WebApp.Extensions.Validation;

public static class ValidationErrorMapper
{
    public static Error[] ToErrors(this FluentValidation.Results.ValidationResult result)
    {
        return result.Errors.Select(e =>
            Error.FromCode(
                code: e.ErrorCode,
                message: e.ErrorMessage,
                type: ErrorTypes.Validation)
                .WithMetadata("property", e.PropertyName)).ToArray();
    }
}

