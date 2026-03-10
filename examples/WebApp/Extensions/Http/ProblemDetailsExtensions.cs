using FunctionalPrimitives;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Extensions.Http;

public static class ProblemDetailsExtensions
{
    extension(IEnumerable<Error> errors)
    {
        public ProblemDetails ToProblemDetails()
        {
            var errorsArr = errors.ToArray();

            var problem = new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "An error occurred",
                Detail = errorsArr.Length > 0 ? errorsArr[0].Message : string.Empty,
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
    }

    extension(Error error)
    {
        public ProblemDetails ToProblemDetails()
        {
            return new[] { error }.ToProblemDetails();
        }
    }
}
