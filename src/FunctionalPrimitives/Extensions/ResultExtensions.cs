namespace FunctionalPrimitives.Extensions;

public static partial class ResultExtensions
{
    extension<T>(T value)
    {
        /// <summary>
        /// Gets a <see cref="Result{T}"/> wrapping this value as a successful result.
        /// </summary>
        public Result<T> AsResult => value;
    }
}
