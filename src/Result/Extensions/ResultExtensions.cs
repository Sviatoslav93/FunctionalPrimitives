namespace Result.Extensions;

public static partial class ResultExtensions
{
    extension<TValue>(TValue value)
    {
        /// <summary>
        /// Gets wraps the value in a Result.
        /// </summary>
        public Result<TValue> AsResult => value;
    }
}
