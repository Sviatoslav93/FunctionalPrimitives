using System.Globalization;
using FunctionalPrimitives.Errors;
using FunctionalPrimitives.Monads.Results;
using FunctionalPrimitives.Monads.Results.Extensions;
using static System.Console;

// Result<T>

// create successful result
WriteLine("Result creation:");

var r1 = 5.ToResult();
var r2 = Success(5);
Result<User> r3 = Success(new User("John", 20));
WriteLine(r1);
WriteLine(r2);
WriteLine(r3);

// create a failed result
var r4 = Failure<int>(new Error("error 1"), new Error("error 2"));
var r5 = Failure<int>("error 3");
WriteLine(r4);
WriteLine(r5);

// to JSON
WriteLine(Success(5).ToJson());
WriteLine(Failure<int>("error-code", "some error message").ToJson());

// binding several results together
WriteLine("Result binding:");
var res1 = Divide(10, 2)
    .Bind(x => Divide(x, 2))
    .Bind(x => Divide(x, 2))
    .Bind(x => Math.Round(x, 1))
    .Bind(x => x.ToString(CultureInfo.InvariantCulture));

res1.Tap(r => WriteLine($"Result: {r}"));

// binding several results together and handling errors
WriteLine("Result binding with errors:");
var res2 = Divide(10, 2)
    .Bind(x => Divide(x, 0)) // stop execution here and return an error
    .Bind(x => Divide(x, 2))
    .Bind(x => Math.Round(x, 1))
    .Bind(x => x.ToString(CultureInfo.InvariantCulture));

res2.TapError(e => WriteLine($"Error: {e}"));

// Example #3 async and success
var res3 = await DivideAsync(10, 2)
    .BindAsync(x => Divide(x, 2))
    .BindAsync(x => DivideAsync(x, 2))
    .BindAsync(x => Math.Round(x, 1))
    .BindAsync(x => x.ToString(CultureInfo.InvariantCulture))
    .MatchAsync(
        value => $"FunctionalPrimitives: {value}",
        errors => $"Errors: {string.Join(", ", errors)}");
WriteLine(res3);

WriteLine(new string('-', 40));

// Example #3 async and failed
var res4 = await DivideAsync(10, 2)
    .BindAsync(x => Divide(x, 0))
    .BindAsync(x => DivideAsync(x, 2))
    .BindAsync(x => Math.Round(x, 1))
    .BindAsync(x => x.ToString(CultureInfo.InvariantCulture))
    .MatchAsync(
        value => $"FunctionalPrimitives: {value}",
        errors => $"Errors: {string.Join(", ", errors)}");
WriteLine(res4);
return;

Result<decimal> Divide(
    decimal a,
    decimal b)
{
    WriteLine($"Divide {a} by {b}");
    if (b == 0)
    {
        return new Error("Division by zero", "code-002");
    }

    return a / b;
}

async Task<Result<decimal>> DivideAsync(
    decimal a,
    decimal b,
    CancellationToken cancellationToken = default)
{
    WriteLine($"Divide {a} by {b}");
    await Task.Delay(1000, cancellationToken);

    if (b == 0)
    {
        return new Error("Division by zero");
    }

    return a / b;
}

#pragma warning disable SA1649
public record User(string Name, int Age);
#pragma warning restore SA1649
