# Result

[![Build Status](https://github.com/Sviatoslav93/Result/workflows/build/badge.svg)](https://github.com/Sviatoslav93/Result/actions)
[![NuGet](https://img.shields.io/nuget/v/Sviatoslav93.Result.svg)](https://www.nuget.org/packages/Sviatoslav93.Result/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

A robust, functional approach to error handling in C#. The Result pattern is an alternative to exception-based error handling that makes error states explicit and composable.

## Features

- 🚀 **Zero exceptions** - Explicit error handling without throwing exceptions
- 🔗 **Fluent chaining** - Chain operations with `Bind`, `BindAsync`, and more
- 🎯 **Type safe** - Compile-time safety for error handling
- ⚡ **High performance** - No exception overhead
- 🧪 **Composable** - Combine, validate, and transform results easily
- 🔄 **Async support** - Full async/await integration
- 📦 **Lightweight** - Minimal dependencies

## Installation

```bash
dotnet add package Sviatoslav93.Result
```

## Quick Start

```csharp
using Result;
using Result.Extensions;

// Basic usage
Result<int> Divide(int a, int b)
{
    if (b == 0)
    {
        return new Error("Division by zero");
    }

    return a / b;
}

// Chain operations
var result = Divide(10, 2)
    .Bind(x => x * 2)
    .Bind(x => x.ToString())
    .Match(
        onSuccess: value => $"Result: {value}",
        onFailure: errors => $"Error: {string.Join(", ", errors)}");

Console.WriteLine(result); // Output: Result: 10
```

## Core Concepts

### Result<T>

The `Result<T>` type represents either a successful value of type `T` or a collection of errors:

```csharp
// Success
Result<string> success = Result.Success("Hello World");
Result<string> implicit = "Hello World"; // Implicit conversion

// Failure
Result<string> failure = Result.Failure<string>(new Error("Something went wrong"));
Result<string> implicitError = new Error("Something went wrong"); // Implicit conversion
```

### Error

The `Error` record represents an error with contextual information:

```csharp
// Simple error
var error = new Error("Invalid input");

// Error with code
var codedError = new Error("Invalid email format", "EMAIL_INVALID");

// Error with exception can be created in the Try method
var result = Result.Try(() => int.Parse("invalid"), ex => new Error(ex.Message));
```

## Usage Examples

### Basic Operations

```csharp
// Creating results
Result<int> success = 42;
Result<int> failure = new Error("Invalid number");

// Checking success/failure
if (result.IsSuccess)
{
    Console.WriteLine($"Value: {result.Value}");
}
else
{
    Console.WriteLine($"Errors: {string.Join(", ", result.Errors)}");
}

// Pattern matching
var message = result.Match(
    onSuccess: value => $"Got: {value}",
    onFailure: errors => $"Failed: {string.Join(", ", errors)}");
```

### Chaining Operations

```csharp
// Synchronous chaining
var result = ParseInt("123")
    .Bind(x => x * 2)
    .Bind(x => x > 100 ? x : new Error("Value too small"))
    .Bind(x => x.ToString());

// Async chaining
var asyncResult = await GetUserAsync(userId)
    .BindAsync(user => GetProfileAsync(user.Id))
    .BindAsync(profile => UpdateLastAccessAsync(profile))
    .MatchAsync(
        onSuccess: profile => $"Updated: {profile.Name}",
        onFailure: errors => $"Failed: {string.Join(", ", errors)}");
```

### Error Handling and Validation

```csharp
// Validation pipeline
Result<string> ValidateEmail(string email)
{
    if (string.IsNullOrWhiteSpace(email))
        return new Error("Email is required");

    if (!email.Contains("@"))
        return new Error("Invalid email format");

    if (email.Length > 100)
        return new Error("Email too long");

    return email;
}

// Chain validations
var userResult = ValidateName(name)
    .Bind(n => ValidateEmail(email)
        .Bind(e => ValidateAge(age)
            .Bind(a => new User(n, e, a))));
```

### Combining Results

```csharp
// Combine multiple results
var nameResult = GetName();
var emailResult = GetEmail();
var ageResult = GetAge();

var userResult = nameResult.Combine(emailResult, ageResult)
    .Bind(values => new User(values[0], values[1], int.Parse(values[2])));

// All must succeed, or you get all errors
if (!userResult.IsSuccess)
{
    foreach (var error in userResult.Errors)
    {
        Console.WriteLine($"Validation error: {error.Message}");
    }
}
```

### Exception Safety

```csharp
// Safely execute code that might throw
var result = Result.Try(() =>
{
    return int.Parse("not-a-number"); // This will throw
});

// result.IsSuccess == false
// result.Errors contains the exception details

// Custom error handling
var customResult = Result.Try(
    () => RiskyOperation(),
    ex => new Error($"Operation failed: {ex.Message}", "RISKY_OP_FAILED"));
```

### Working with None Type

For operations that don't return values:

```csharp
// Use None for void operations
Result<None> SaveData(string data)
{
    try
    {
        File.WriteAllText("data.txt", data);
        return None.Value;
    }
    catch (Exception ex)
    {
        return new Error("Failed to save data");
    }
}

// Chain void operations
var saveResult = await ValidateData(data)
    .BindAsync(d => SaveDataAsync(d))
    .BindAsync(_ => LogSuccessAsync())
    .BindAsync(_ => NotifyUsersAsync());
```

### Advanced Patterns

#### Recovery and Fallbacks

```csharp
// Provide fallback values
var result = GetPrimaryData()
    .Recover("default-value")
    .Recover(errors => $"Fallback for: {errors.Count()} errors");

// Conditional recovery
var recovered = GetUserPreferences()
    .Recover(errors =>
        errors.Any(e => e.Code == "NOT_FOUND")
            ? "default-preferences"
            : throw new InvalidOperationException());
```

#### Side Effects with Do

```csharp
// Perform side effects without changing the result
var result = await ProcessDataAsync(input)
    .DoAsync(data => LogProcessedAsync(data))
    .DoAsync(data => CacheResultAsync(data))
    .DoErrorAsync(errors => LogErrorsAsync(errors))
    .BindAsync(data => TransformAsync(data));
```

## Async Patterns

### Task<Result<T>> Integration

```csharp
// Working with Task<Result<T>>
Task<Result<User>> GetUserAsync(int id);

var result = await GetUserAsync(123)
    .BindAsync(user => GetProfileAsync(user.Id))
    .BindAsync(profile => EnrichProfileAsync(profile))
    .MatchAsync(
        onSuccess: profile => $"Welcome {profile.DisplayName}",
        onFailure: errors => "Failed to load profile");
```

## Best Practices

### 1. Use Implicit Conversions

```csharp
// Good: Clean and readable
Result<string> GetUserName() => "John Doe";
Result<string> GetError() => new Error("User not found");

// Avoid: Verbose
Result<string> GetUserNameVerbose() => Result.Success("John Doe");
```

### 2. Chain Operations for Readability

```csharp
// Good: Fluent pipeline
var result = input
    .AsResult()
    .Bind(Validate)
    .Bind(Transform)
    .Bind(Save)
    .Match(
        onSuccess: data => $"Saved: {data.Id}",
        onFailure: errors => LogAndReturnError(errors));
```

### 3. Use Meaningful Error Messages and Codes

```csharp
// Good: Descriptive errors with codes
if (age < 0)
    return new Error("Age cannot be negative", "INVALID_AGE");

// Avoid: Generic errors
if (age < 0)
    return new Error("Invalid input");
```

### 4. Combine Multiple Results

```csharp
// Combine multiple results into one
var nameResult = GetName();
var emailResult = GetEmail();
var ageResult = GetAge();

var userResult = nameResult.Combine(emailResult, ageResult)
    .Bind(values => new User(values[0], values[1], int.Parse(values[2])));
```

## Performance

The Result pattern eliminates exception overhead:

```csharp
// Traditional exception-based (slow for expected failures)
try
{
    var result = int.Parse(userInput);
    return result * 2;
}
catch (FormatException)
{
    return -1; // Default value
}

// Result-based (fast)
return Result.Try(() => int.Parse(userInput))
    .Recover(-1)
    .Match(
        x => x * 2,
        _ => -1);
```

## Testing

Results make testing error conditions straightforward:

```csharp
[Fact]
public void Should_Return_Error_When_Division_By_Zero()
{
    var result = Calculator.Divide(10, 0);

    Assert.False(result.IsSuccess);
    Assert.Single(result.Errors);
    Assert.Contains("division by zero", result.Errors[0].Message);
}

[Fact]
public void Should_Chain_Successful_Operations()
{
    var result = Calculator.Divide(10, 2)
        .Bind(x => x * 3)
        .Bind(x => x.ToString());

    Assert.True(result.IsSuccess);
    Assert.Equal("15", result.Value);
}
```

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.
