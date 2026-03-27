using FunctionalPrimitives.Errors;

namespace FunctionalPrimitives.Monads.Results.Extensions;

public static partial class ResultExtensions
{
    /// <param name="result">The result object on which the operation is performed.</param>
    /// <typeparam name="T">The type of the value contained within the result.</typeparam>
    extension<T>(Result<T> result)
    {
        /// <summary>
        /// Executes the specified action if the result is successful.
        /// If the result is a failure, execution of the action is skipped, and the original result is returned.
        /// </summary>
        /// <param name="action">The action to execute when the result is successful.</param>
        /// <returns>The original result, whether the action is executed or not.</returns>
        public Result<T> Tap(Action<T> action)
        {
            return result.Match(
                x =>
                {
                    action(x);
                    return x;
                },
                _ => result);
        }

        /// <summary>
        /// Executes the specified action if the result is successful.
        /// If the result is a failure, the action is not executed, and the original result is returned.
        /// </summary>
        /// <param name="action">The action to execute when the result is successful.</param>
        /// <returns>The original result, whether the action is executed or not.</returns>
        public Result<T> Tap(Action action)
        {
            return result.Bind(x =>
            {
                action();
                return x;
            });
        }

        public Result<T> TapError(Action action)
        {
            if (!result.IsSuccess)
            {
                action();
            }

            return result;
        }

        /// <summary>
        /// Executes the specified action if the result contains errors.
        /// The action is invoked with the collection of errors from the result.
        /// </summary>
        /// <param name="action">The action to execute when the result is not successful, receiving the errors as input.</param>
        /// <returns>The original result, unmodified.</returns>
        public Result<T> TapError(Action<IEnumerable<Error>> action)
        {
            if (!result.IsSuccess)
            {
                action(result.Errors);
            }

            return result;
        }
    }
}
