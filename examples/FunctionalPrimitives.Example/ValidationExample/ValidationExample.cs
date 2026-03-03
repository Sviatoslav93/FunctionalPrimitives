using FunctionalPrimitives.Extensions;

namespace FunctionalPrimitives.Example;

public static class ValidationExample
{
    public static void Example()
    {
        var address = Address.Create("123 Main St", "Anytown", "CA", "12345");

        address.Tap(x => Console.WriteLine($"Address: {x}"))
            .TapError(x =>
            {
                foreach (var error in x)
                {
                    Console.WriteLine($"Error: {error.Message}");
                }
            });
    }
}
