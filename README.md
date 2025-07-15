# Result

[![Build Status](https://github.com/Sviatoslav93/Result/workflows/build/badge.svg)](https://github.com/Sviatoslav93/Result/actions)
[![NuGet](https://img.shields.io/nuget/v/Sviatoslav93.Result.svg)](https://www.nuget.org/packages/Sviatoslav93.Result/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

A robust, functional approach to error handling in C#. The Result pattern is an alternative to exception-based error handling that makes error states explicit and composable.

## Features

- 🚀 **Zero exceptions** - Explicit error handling without throwing exceptions
- 🔗 **Fluent chaining** - Chain operations with `Then`, `ThenAsync`, and more
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
        return new Error("Division by zero");

    return a / b;
}

// Chain operations
var result = Divide(10, 2)
    .Then(x => x * 2)
    .Then(x => x.ToString())
    .Match(
        onSuccess: value => $"Result: {value}",
        onFailure: errors => $"Error: {string.Join(", ", errors)}");

Console.WriteLine(result); // "Result: 10"
```

## Core Concepts

### Result<T>

The `Result<T>` type represents either a successful value of type `T` or a collection of errors:

```csharp
// Success
Result<string> success = Result<string>.Success("Hello World");
Result<string> implicit = "Hello World"; // Implicit conversion

// Failure
Result<string> failure = Result<string>.Failed(new Error("Something went wrong"));
Result<string> implicitError = new Error("Something went wrong"); // Implicit conversion
```

### Error Types

Create rich error information:

```csharp
// Simple error
var error = new Error("Invalid input");

// Error with code
var codedError = Error.Create("Invalid email format", "EMAIL_INVALID");

// Error with exception
var exceptionError = Error.Create("Database error", ex);

// Error with metadata
var richError = Error.Create("Validation failed", "VALIDATION_ERROR")
    .WithMetadata("field", "email")
    .WithMetadata("value", "invalid-email");
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
    .Then(x => x * 2)
    .Then(x => x > 100 ? x : throw new InvalidOperationException())
    .Then(x => x.ToString());

// Async chaining
var asyncResult = await GetUserAsync(userId)
    .ThenAsync(user => GetProfileAsync(user.Id))
    .ThenAsync(profile => UpdateLastAccessAsync(profile))
    .MatchAsync(
        onSuccess: profile => $"Updated: {profile.Name}",
        onFailure: errors => $"Failed: {string.Join(", ", errors)}");
```

### Error Handling and Validation

```csharp
// Validation pipeline
Result<string> ValidateEmail(string email)
{
    return email.ToResult()
        .EnsureNotNullOrWhiteSpace(Error.Create("Email is required"))
        .Ensure(x => x.Contains("@"), Error.Create("Invalid email format"))
        .Ensure(x => x.Length <= 100, Error.Create("Email too long"));
}

// Multiple validations
var userResult = CreateUser(name, email, age)
    .EnsureAll(
        (user => !string.IsNullOrEmpty(user.Name), Error.Create("Name required")),
        (user => user.Age >= 18, Error.Create("Must be 18 or older")),
        (user => IsValidEmail(user.Email), Error.Create("Invalid email"))
    );
```

### Combining Results

```csharp
// Combine multiple results
var nameResult = GetName();
var emailResult = GetEmail();
var ageResult = GetAge();

var userResult = ResultExtensions.Combine(nameResult, emailResult, ageResult)
    .Then(values => new User(values[0], values[1], int.Parse(values[2])));

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
var result = ResultExtensions.Try(() =>
{
    return int.Parse("not-a-number"); // This will throw
});

// result.IsSuccess == false
// result.Errors contains the exception details

// Custom error handling
var customResult = ResultExtensions.Try(
    () => RiskyOperation(),
    ex => Error.Create($"Operation failed: {ex.Message}", "RISKY_OP_FAILED", ex));
```

### Working with Unit Type

For operations that don't return values:

```csharp
// Use Unit for void operations
Result<Unit> SaveData(string data)
{
    try
    {
        File.WriteAllText("data.txt", data);
        return Unit.Value;
    }
    catch (Exception ex)
    {
        return Error.Create("Failed to save data", ex);
    }
}

// Chain void operations
var saveResult = await ValidateData(data)
    .ThenAsync(d => SaveDataAsync(d))
    .ThenAsync(_ => LogSuccessAsync())
    .ThenAsync(_ => NotifyUsersAsync());
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
            ? GetDefaultPreferences()
            : Result<Preferences>.Failed(errors));
```

#### Side Effects with Tap

```csharp
// Perform side effects without changing the result
var result = await ProcessDataAsync(input)
    .TapAsync(data => LogProcessedAsync(data))
    .TapAsync(data => CacheResultAsync(data))
    .TapErrorAsync(errors => LogErrorsAsync(errors))
    .ThenAsync(data => TransformAsync(data));
```

#### Filtering and Conditional Logic

```csharp
// Filter results based on conditions
var result = GetNumbers()
    .Where(numbers => numbers.Length > 0, Error.Create("No numbers provided"))
    .Where(numbers => numbers.All(n => n > 0), Error.Create("All numbers must be positive"));
```

## Async Patterns

### Task<Result<T>> Integration

```csharp
// Working with Task<Result<T>>
Task<Result<User>> GetUserAsync(int id);

var result = await GetUserAsync(123)
    .ThenAsync(user => GetProfileAsync(user.Id))
    .ThenAsync(profile => EnrichProfileAsync(profile))
    .MatchAsync(
        onSuccess: profile => $"Welcome {profile.DisplayName}",
        onFailure: errors => "Failed to load profile");
```

### Parallel Operations

```csharp
// Combine multiple async operations
var tasks = new[]
{
    GetUserAsync(1),
    GetUserAsync(2),
    GetUserAsync(3)
};

var combinedResult = await ResultExtensions.CombineAsync(tasks);
// Success: Result<User[]> with all users
// Failure: Result<User[]> with all errors from failed operations
```

## Best Practices

### 1. Use Implicit Conversions

```csharp
// Good: Clean and readable
Result<string> GetUserName() => "John Doe";
Result<string> GetError() => new Error("User not found");

// Avoid: Verbose
Result<string> GetUserNameVerbose() => Result<string>.Success("John Doe");
```

### 2. Chain Operations for Readability

```csharp
// Good: Fluent pipeline
var result = input
    .ToResult()
    .Then(Validate)
    .Then(Transform)
    .Then(Save)
    .Match(
        onSuccess: data => $"Saved: {data.Id}",
        onFailure: errors => LogAndReturnError(errors));
```

### 3. Use Meaningful Error Messages

```csharp
// Good: Descriptive errors
if (age < 0)
    return Error.Create("Age cannot be negative", "INVALID_AGE")
        .WithMetadata("provided_age", age);

// Avoid: Generic errors
if (age < 0)
    return new Error("Invalid input");
```

### 4. Combine Related Validations

```csharp
// Collect all validation errors
var userResult = CreateUser(dto)
    .EnsureAll(
        (u => !string.IsNullOrEmpty(u.Name), Error.Create("Name required")),
        (u => IsValidEmail(u.Email), Error.Create("Invalid email")),
        (u => u.Age >= 18, Error.Create("Must be adult"))
    );
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
return TryParse(userInput)
    .Then(x => x * 2)
    .GetValueOrDefault(-1);
```

## Testing

Results make testing error conditions straightforward:

```csharp
[Test]
public void Should_Return_Error_When_Division_By_Zero()
{
    var result = Calculator.Divide(10, 0);

    Assert.False(result.IsSuccess);
    Assert.Contains(result.Errors, e => e.Message.Contains("division by zero"));
}

[Test]
public void Should_Chain_Successful_Operations()
{
    var result = Calculator.Divide(10, 2)
        .Then(x => x * 3)
        .Then(x => x.ToString());

    Assert.True(result.IsSuccess);
    Assert.Equal("15", result.Value);
}
```

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.
