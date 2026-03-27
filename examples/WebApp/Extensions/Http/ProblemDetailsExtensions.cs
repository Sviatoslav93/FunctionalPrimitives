using FunctionalPrimitives.Errors;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Extensions.Http;

public static class ProblemDetailsExtensions
{
    public static ProblemDetails ToProblemDetails(this IEnumerable<Error> errors)
    {
        var errorsArr = errors.ToArray();

        var firstError = errorsArr.FirstOrDefault();
        var statusCode = firstError switch
        {
            ConflictError => StatusCodes.Status409Conflict,
            ForbiddenError => StatusCodes.Status403Forbidden,
            InvalidStateError => StatusCodes.Status400BadRequest,
            NotFoundError => StatusCodes.Status404NotFound,
            TimeoutError => StatusCodes.Status408RequestTimeout,
            UnauthorizedError => StatusCodes.Status401Unauthorized,
            UnexpectedError => StatusCodes.Status500InternalServerError,
            ValidationError => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError,
        };

        var isMultipleErrors = errorsArr.Length > 1;
        var problem = new ProblemDetails
        {
            Status = statusCode,
            Title = isMultipleErrors ? "Multiple errors occurred" : firstError?.Message,
            Detail = isMultipleErrors ? string.Empty : firstError?.Message,
            Extensions =
            {
                ["errors"] = errorsArr
                    .Select(x => new
                    {
                        x.Code,
                        x.Message,
                    })
                    .ToArray(),
            },
        };

        return problem;
    }

    public static ProblemDetails ToProblemDetails(this Error error)
    {
        return new[] { error }.ToProblemDetails();
    }
}
