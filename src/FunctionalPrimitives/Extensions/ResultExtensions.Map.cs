namespace FunctionalPrimitives.Extensions;

public static partial class ResultExtensions
{
    extension<TValue>(Result<TValue> result)
    {
        public Result<TNext> Map<TNext>(Func<TValue, TNext> mapper)
        {
            return result.IsSuccess
                ? mapper(result.Value)
                : result.Errors.ToArray();
        }

        public Result<TNext> Select<TNext>(
            Func<TValue, TNext> selector)
            => result.Map(selector);
    }
}
