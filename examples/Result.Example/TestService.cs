using System.Collections;

namespace Result.Example;

public static partial class TestService
{
    public static Result<decimal> Divide(decimal a, decimal b)
    {
        Console.WriteLine($"Divide {a} by {b}");
        if (b == 0)
        {
            return new Error("Division by zero");
        }

        return a / b;
    }

    public static async Task<Result<decimal>> DivideAsync(decimal a, decimal b, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"Divide {a} by {b}");
        await Task.Delay(1000, cancellationToken);

        if (b == 0)
        {
            return new Error("Division by zero");
        }

        return a / b;
    }
}
