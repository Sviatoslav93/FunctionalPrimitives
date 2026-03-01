using FunctionalPrimitives.Example;
using FunctionalPrimitives.Extensions.Result;

// Example #1 sync and success
var res1 = TestService.Divide(10, 2)
    .Bind(x => TestService.Divide(x, 2))
    .Bind(x => TestService.Divide(x, 2))
    .Bind(x => Math.Round(x, 1))
    .Bind(x => x.ToString())
    .Match(
        value => $"FunctionalPrimitives: {value}",
        errors => $"Errors: {string.Join(", ", errors)}");
Console.WriteLine(res1);

Console.WriteLine(new string('-', 40));

// Example #2 sync and failed
var res2 = TestService.Divide(10, 2)
    .Bind(x => TestService.Divide(x, 0)) // stop execution here and return an error
    .Bind(x => TestService.Divide(x, 2))
    .Bind(x => Math.Round(x, 1))
    .Bind(x => x.ToString())
    .Match(
        value => $"FunctionalPrimitives: {value}",
        errors => $"Errors: {string.Join(", ", errors)}");
Console.WriteLine(res2);

Console.WriteLine(new string('-', 40));

// Example #3 async and success
var res3 = await TestService.DivideAsync(10, 2)
    .BindAsync(x => TestService.Divide(x, 2))
    .BindAsync(x => TestService.DivideAsync(x, 2))
    .BindAsync(x => Math.Round(x, 1))
    .BindAsync(x => x.ToString())
    .MatchAsync(
        value => $"FunctionalPrimitives: {value}",
        errors => $"Errors: {string.Join(", ", errors)}");
Console.WriteLine(res3);

Console.WriteLine(new string('-', 40));

// Example #3 async and failed
var res4 = await TestService.DivideAsync(10, 2)
    .BindAsync(x => TestService.Divide(x, 0))
    .BindAsync(x => TestService.DivideAsync(x, 2))
    .BindAsync(x => Math.Round(x, 1))
    .BindAsync(x => x.ToString())
    .MatchAsync(
        value => $"FunctionalPrimitives: {value}",
        errors => $"Errors: {string.Join(", ", errors)}");
Console.WriteLine(res4);
