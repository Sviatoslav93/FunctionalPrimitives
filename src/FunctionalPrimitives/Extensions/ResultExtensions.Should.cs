namespace FunctionalPrimitives.Extensions;

public static partial class ResultExtensions
{
    extension<TValue>(TValue value)
    {
        public Result<TValue> Should(Func<TValue, bool> predicate, Error error)
        {
            return predicate(value) ? value : error;
        }
    }

    extension<TValue>(Result<TValue> result)
    {
        public Result<TValue> Should(Func<TValue, bool> predicate, Error error)
        {
            if (result.IsFailure) return result;

            return predicate(result.Value) ? result : error;
        }
    }
}
