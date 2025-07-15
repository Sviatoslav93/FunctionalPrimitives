namespace Result.Extensions;

public static partial class ResultExtensions
{
    public static Result<TNextValue> Then<TValue, TNextValue>(
        this Result<TValue> result,
        Func<TValue, TNextValue> onSuccess)
    {
        return result.IsSuccess
            ? onSuccess(result.Value)
            : result.Errors;
    }

    public static Result<TNextValue> Then<TValue, TNextValue>(
        this Result<TValue> result,
        Func<TValue, Result<TNextValue>> onSuccess)
    {
        return result.IsSuccess
            ? onSuccess(result.Value)
            : result.Errors;
    }
}
