using FunctionalPrimitives;
using FunctionalPrimitives.Extensions.Result;
using HttpResult = Microsoft.AspNetCore.Http.Results;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace WebApp.Extensions.Http;

public static class HttpResultExtensions
{
    extension<T>(Task<Result<T>> result)
    {
        public Task<IResult> ToHttpResultAsync(Func<T, IResult>? onSuccess = null)
        {
            onSuccess ??= HttpResult.Ok;

            return result.MatchAsync(
                onSuccess,
                errors => HttpResult.Problem(errors.ToProblemDetails()));
        }
    }

    extension<T>(Result<T> result)
    {
        public IResult ToHttpResult(Func<T, IResult>? onSuccess = null)
        {
            onSuccess ??= HttpResult.Ok;

            return result.Match(
                onSuccess,
                errors => HttpResult.Problem(errors.ToProblemDetails()));
        }
    }
}
