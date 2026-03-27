using FunctionalPrimitives.Errors;
using FunctionalPrimitives.Monads.Results;
using FunctionalPrimitives.Monads.Results.Extensions;
using Shouldly;
using Xunit;

namespace FunctionalPrimitives.Tests.Monads.Results.Extensions;

public partial class ResultExtensionsTests
{
    [Fact]
    public void Recover_WithFallbackValue_ShouldReturnOriginalValue_WhenResultIsSuccess()
    {
        // Arrange
        var originalValue = 42;
        var fallbackValue = 0;
        var result = Result.Success(originalValue);

        // Act
        var recovered = result.Recover(fallbackValue);

        // Assert
        recovered.IsSuccess.ShouldBeTrue();
        recovered.Value.ShouldBe(originalValue);
    }

    [Fact]
    public void Recover_WithFallbackValue_ShouldReturnFallbackValue_WhenResultIsFailure()
    {
        // Arrange
        var fallbackValue = 99;
        var result = Result.Failure<int>(new Error("Something went wrong"));

        // Act
        var recovered = result.Recover(fallbackValue);

        // Assert
        recovered.IsSuccess.ShouldBeTrue();
        recovered.Value.ShouldBe(fallbackValue);
    }

    [Fact]
    public void Recover_WithRecoveryFactory_ShouldReturnOriginalValue_WhenResultIsSuccess()
    {
        // Arrange
        var originalValue = "success";
        var result = Result.Success(originalValue);
        var factoryCalled = false;

        // Act
        var recovered = result.Recover(errors =>
        {
            factoryCalled = true;
            return "fallback";
        });

        // Assert
        recovered.IsSuccess.ShouldBeTrue();
        recovered.Value.ShouldBe(originalValue);
        factoryCalled.ShouldBeFalse();
    }

    [Fact]
    public void Recover_WithRecoveryFactory_ShouldInvokeFactoryAndReturnValue_WhenResultIsFailure()
    {
        // Arrange
        var error1 = new Error("Error 1", "CODE1");
        var error2 = new Error("Error 2", "CODE2");
        var result = Failure<string>(error1, error2);
        IEnumerable<Error>? capturedErrors = null;

        // Act
        var recovered = result.Recover(errors =>
        {
            capturedErrors = errors.ToList();
            return "recovered value";
        });

        // Assert
        recovered.IsSuccess.ShouldBeTrue();
        recovered.Value.ShouldBe("recovered value");
        capturedErrors.ShouldNotBeNull();
        capturedErrors.Count().ShouldBe(2);
        capturedErrors.ShouldContain(error1);
        capturedErrors.ShouldContain(error2);
    }

    [Fact]
    public void Recover_WithRecoveryFactory_ShouldProvideErrorsToFactory()
    {
        // Arrange
        var errorMessage = "Critical failure";
        var errorCode = "ERR_500";
        var error = new Error(errorMessage, errorCode);
        var result = Result.Failure<int>(error);

        // Act
        var recovered = result.Recover(errors =>
        {
            var firstError = errors.First();
            return firstError.Code == errorCode ? -1 : 0;
        });

        // Assert
        recovered.IsSuccess.ShouldBeTrue();
        recovered.Value.ShouldBe(-1);
    }

    [Fact]
    public void Recover_WithFallbackValue_ShouldHandleNullFallbackValue()
    {
        // Arrange
        var result = Result.Failure<string?>(new Error("Error"));

        // Act
        var recovered = result.Recover((string?)null);

        // Assert
        recovered.IsSuccess.ShouldBeTrue();
        recovered.Value.ShouldBeNull();
    }

    [Fact]
    public void Recover_WithRecoveryFactory_ShouldHandleNullReturnValue()
    {
        // Arrange
        var result = Result.Failure<string?>(new Error("Error"));

        // Act
        var recovered = result.Recover(_ => null);

        // Assert
        recovered.IsSuccess.ShouldBeTrue();
        recovered.Value.ShouldBeNull();
    }
}
