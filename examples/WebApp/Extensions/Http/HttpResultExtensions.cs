using FunctionalPrimitives.Monads.Results;
using FunctionalPrimitives.Monads.Results.Extensions;
using HttpResult = Microsoft.AspNetCore.Http.Results;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace WebApp.Extensions.Http;

public static class HttpResultExtensions
{
    public static Task<IResult> ToHttpResultAsync<T>(this Task<Result<T>> result, Func<T, IResult>? onSuccess = null)
    {
        onSuccess ??= HttpResult.Ok;

        return result.MatchAsync(
            onSuccess,
            errors => HttpResult.Problem(errors.ToProblemDetails()));
    }

    public static IResult ToHttpResult<T>(this Result<T> result, Func<T, IResult>? onSuccess = null)
    {
        onSuccess ??= HttpResult.Ok;

        return result.Match(
            onSuccess,
            errors => HttpResult.Problem(errors.ToProblemDetails()));
    }
}
