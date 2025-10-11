namespace Result;

public partial class Result
{
    /// <summary>
    /// Creates a successful result.
    /// </summary>
    /// <returns>Successful result.</returns>
    public static Result Success() => new();

    /// <summary>
    /// Creates a failed result.
    /// </summary>
    /// <param name="error">An <see cref="Error"/>.</param>
    /// <returns>A failed <see cref="Result"/> instance.</returns>
    public static Result Failure(Error error) => error;

    public static Result Failure(params Error[] errors) => errors;

    public static Result Failure(IEnumerable<Error> errors)
    {
        var arr = errors as Error[] ?? [.. errors];

        if (errors == null || arr.Length == 0)
        {
            throw new ArgumentException("At least one error must be provided.", nameof(errors));
        }

        return arr.ToArray();
    }

    public static Result Try(Action action, Func<Exception, Error> error)
    {
        try
        {
            action();
            return Success();
        }
        catch (Exception ex)
        {
            return Failure(error(ex));
        }
    }
}
