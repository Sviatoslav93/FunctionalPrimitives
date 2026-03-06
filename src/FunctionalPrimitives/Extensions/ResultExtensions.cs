namespace FunctionalPrimitives.Extensions;

public static partial class ResultExtensions
{
    extension<T>(T value)
    {
        /// <summary>
        /// Gets wraps the value in a FunctionalPrimitives.
        /// </summary>
        public Result<T> AsResult => value;
    }
}
