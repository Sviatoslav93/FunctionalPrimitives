using FunctionalPrimitives.Extensions;

using static FunctionalPrimitives.Result;

namespace FunctionalPrimitives.Example.ResultExamples;

public partial class ResultExample
{
    public void Do()
    {
        var result = from x in Parse("42")
            from y in Parse("0")
            from r1 in Divide(x, y)
            from z in Parse("33")
            select r1 + z;

        result.Tap(x => Console.WriteLine(x));
        result.TapError(x =>
        {
            foreach (var error in x)
            {
                Console.WriteLine(error.Message);
            }
        });
    }

    private static Result<int> Parse(string input)
    {
        return int.TryParse(input, out var result)
            ? Success(result)
            : Failure<int>(new Error("parse"));
    }

    private static Result<int> Divide(int x, int y)
    {
        return y == 0
            ? Failure<int>(new Error("division by zero"))
            : Success(x / y);
    }
}
