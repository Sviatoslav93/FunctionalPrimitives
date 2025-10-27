using Shouldly;
using Xunit;

namespace Result.Tests;

public class ResultTests
{
    [Fact]
    public void ImplicitConversion_FromValue_CreatesSuccessResult()
    {
        // Arrange & Act
        Result<int> result = 42;

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(42, result.Value);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void ImplicitConversion_FromError_CreatesFailureResult()
    {
        // Arrange
        var error = new Error("Test.Error", "Test error message");

        // Act
        Result<int> result = error;

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Single(result.Errors);
        Assert.Contains(error, result.Errors);
    }

    [Fact]
    public void ImplicitConversion_FromErrorArray_CreatesFailureResult()
    {
        // Arrange
        var errors = new[]
        {
            new Error("Test.Error1", "First error"),
            new Error("Test.Error2", "Second error"),
        };

        // Act
        Result<int> result = errors;

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(2, result.Errors.Count());
        Assert.Contains(errors[0], result.Errors);
        Assert.Contains(errors[1], result.Errors);
    }

    [Fact]
    public void Value_WhenSuccess_ReturnsValue()
    {
        // Arrange
        Result<string> result = "test value";

        // Act
        var value = result.Value;

        // Assert
        Assert.Equal("test value", value);
    }

    [Fact]
    public void Value_WhenFailure_ThrowsInvalidOperationException()
    {
        // Arrange
        Result<int> result = new Error("Test.Error", "Test error");

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => result.Value);
    }

    [Fact]
    public void Deconstruct_WhenSuccess_ReturnsValueAndEmptyErrors()
    {
        // Arrange
        Result<int> result = 100;

        // Act
        var (value, errors) = result;

        // Assert
        Assert.Equal(100, value);
        Assert.Empty(errors);
    }

    [Fact]
    public void Deconstruct_WhenFailure_ReturnsNullValueAndErrors()
    {
        // Arrange
        var error = new Error("Test.Error", "Test error");
        Result<int> result = error;

        // Act
        var (value, errors) = result;

        // Assert
        Assert.Equal(0, value);
        var collection = errors as Error[] ?? errors.ToArray();
        Assert.Single(collection);
        Assert.Contains(error, collection);
    }

    [Fact]
    public void GenericResult_WithReferenceType_WorksCorrectly()
    {
        // Arrange & Act
        Result<string> result = "hello";

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("hello", result.Value);
    }

    [Fact]
    public void GenericResult_WithValueType_WorksCorrectly()
    {
        // Arrange & Act
        Result<double> result = 3.14;

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(3.14, result.Value);
    }

    [Fact]
    public void Should_CreateSuccessGenericResult()
    {
        var res1 = Result.Success(22);
        var res2 = Result.Success("str");
        var res3 = Result.Success<int[]>([]);

        res1.IsSuccess.ShouldBeTrue();
        res2.IsSuccess.ShouldBeTrue();
        res3.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public void Should_CreateSuccessResultForNullableValueType()
    {
        var result = Result.Success<int?>(null);

        result.Value.ShouldBeNull();
        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public void Should_CreateSuccessResultForReferenceType()
    {
        var obj = new object();

        var result = Result.Success(obj);

        result.Value.ShouldNotBeNull();
        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public void Should_CreateSuccessResultForNullableReferenceType()
    {
        var testDto = default(TestUserDto);
        var result = Result.Success(testDto);

        result.Value.ShouldBeNull();
        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public void Should_CreateSuccessResultForNullableReferenceTypeWithNull()
    {
        var result = Result.Success<TestUserDto?>(null);

        result.Value.ShouldBeNull();
        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public void Should_CreateFailedResult()
    {
        var result = Result.Failure<string>(new Error("test error"));

        result.IsSuccess.ShouldBeFalse();
        result.Errors.ShouldNotBeEmpty();
    }

    [Fact]
    public void Should_CreateFailedResult_WithErrors()
    {
        var result = Result.Failure<string>(new Error("test error"), new Error("test error 2"));

        result.IsSuccess.ShouldBeFalse();
        result.Errors.ShouldNotBeEmpty();
        result.Errors.Count().ShouldBe(2);
    }

    [Fact]
    public void Should_ReturnSuccessResult_WhenTrySuccess()
    {
        var result = Result.Try(() => new object());

        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public void Should_ReturnFailedResult_WhenTryFailure()
    {
        var result = Result.Try<object>(() => throw new Exception("test exception"));

        result.IsSuccess.ShouldBeFalse();
        result.Errors.ShouldNotBeEmpty();
    }

    [Fact]
    public async Task Should_ReturnSuccess_WhenTryAsync()
    {
        var result = await Result.TryAsync<Result<string>>(async () => await Task.FromResult<string>("str"));

        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task Should_ReturnFailedResult_WhenTryAsyncFailure()
    {
        var result = await Result.TryAsync<Result<string>>(async () =>
        {
            await Task.Delay(1);
            throw new Exception("test exception");
        });

        result.IsSuccess.ShouldBeFalse();
        result.Errors.ShouldNotBeEmpty();
    }

    private class TestUserDto
    {
        public required string Email { get; set; }

        public required string FullName { get; set; }
    }
}
