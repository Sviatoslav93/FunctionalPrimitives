namespace FunctionalPrimitives.Extensions;

public partial class ResultExtensions
{
    extension<T>(Task<Result<T>> resultTask)
    {
        public Task<Result<U>> MapAsync<U>(Func<T, U> projection)
        {
            return resultTask.MatchAsync(
                x => projection(x),
                Failure<U>);
        }

        public Task<Result<U>> Select<U>(Func<T, U> projection)
            => resultTask.MapAsync(projection);
    }
}
