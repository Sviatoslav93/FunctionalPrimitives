using System.Globalization;
using Result.Example;
using Result.Extensions;

// Example #1 sync and success
var res1 = TestService.Divide(10, 2)
    .Then(x => TestService.Divide(x, 2))
    .Then(x => TestService.Divide(x, 2))
    .Then(x => Math.Round(x, 1))
    .Then(x => x.ToString(CultureInfo.InvariantCulture))
    .Match(
        value => $"Result: {value}",
        errors => $"Errors: {string.Join(", ", errors)}");
Console.WriteLine(res1);

Console.WriteLine(new string('-', 40));

// Example #2 sync and failed
var res2 = TestService.Divide(10, 2)
    .Then(x => TestService.Divide(x, 0)) // stop execution here and return an error
    .Then(x => TestService.Divide(x, 2))
    .Then(x => Math.Round(x, 1))
    .Then(x => x.ToString(CultureInfo.InvariantCulture))
    .Match(
        value => $"Result: {value}",
        errors => $"Errors: {string.Join(", ", errors)}");
Console.WriteLine(res2);

Console.WriteLine(new string('-', 40));

// Example #3 async and success
var res3 = await TestService.DivideAsync(10, 2)
    .ThenAsync(x => TestService.Divide(x, 2))
    .ThenAsync(x => TestService.DivideAsync(x, 2))
    .ThenAsync(x => Math.Round(x, 1))
    .ThenAsync(x => x.ToString(CultureInfo.InvariantCulture))
    .MatchAsync(
        value => $"Result: {value}",
        errors => $"Errors: {string.Join(", ", errors)}");
Console.WriteLine(res3);

Console.WriteLine(new string('-', 40));

// Example #3 async and failed
var res4 = await TestService.DivideAsync(10, 2)
    .ThenAsync(x => TestService.Divide(x, 0))
    .ThenAsync(x => TestService.DivideAsync(x, 2))
    .ThenAsync(x => Math.Round(x, 1))
    .ThenAsync(x => x.ToString(CultureInfo.InvariantCulture))
    .MatchAsync(
        value => $"Result: {value}",
        errors => $"Errors: {string.Join(", ", errors)}");
Console.WriteLine(res4);
