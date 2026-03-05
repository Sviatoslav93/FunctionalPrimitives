using FunctionalPrimitives.Extensions;

using static FunctionalPrimitives.Maybe;

namespace FunctionalPrimitives.Example.MaybeExamples;

public class MaybeExample
{
    public void Do()
    {
        var result = from x in TryParse("42")
            from y in TryParse("abc")
            select x + y;

        result.Tap(Console.WriteLine);
        result.TapNone(() => Console.WriteLine("None"));
    }

    private static Maybe<int> TryParse(string input)
    {
        return int.TryParse(input, out var result)
            ? Some(result)
            : None<int>();
    }
}
