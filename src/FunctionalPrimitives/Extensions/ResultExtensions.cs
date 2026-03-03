namespace FunctionalPrimitives.Extensions;

public static partial class ResultExtensions
{
    extension<TValue>(TValue value)
    {
        /// <summary>
        /// Gets wraps the value in a FunctionalPrimitives.
        /// </summary>
        public Result<TValue> AsResult => value;
    }
}
