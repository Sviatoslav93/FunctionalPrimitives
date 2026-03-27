using FunctionalPrimitives.Errors;
using FunctionalPrimitives.Monads.Results;
using Shouldly;
using Xunit;
using Exception = System.Exception;

namespace FunctionalPrimitives.Tests.Monads.Results;

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
        result.Deconstruct(out var isSuccess, out var deconstructed);

        // Assert
        isSuccess.ShouldBeTrue();
        deconstructed.Value.ShouldBe(100);
        deconstructed.Errors.ShouldBeEmpty();
    }

    [Fact]
    public void Deconstruct_WhenFailure_ReturnsNullValueAndErrors()
    {
        // Arrange
        var error = new Error("Test.Error", "Test error");
        Result<int> result = error;

        // Act
        result.Deconstruct(out var isSuccess, out var deconstructed);

        // Assert
        isSuccess.ShouldBeFalse();
        deconstructed.Value.ShouldBe(default);
        deconstructed.Errors.ShouldHaveSingleItem();
        deconstructed.Errors.ShouldContain(error);
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
    public void Should_UseEmptyError_WhenNoErrorsProvided()
    {
        var res = Result.Failure<string>();

        res.IsSuccess.ShouldBeFalse();
        res.Errors.First().ShouldBe(Error.Empty);
    }

    [Fact]
    public void Should_ThrowException_WhenNoErrorsProvided()
    {
        Action action = () =>
        {
            Result<int> res = new Result<int>([]);
        };

        action.ShouldThrow<ArgumentException>();
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

    [Fact]
    public void IsFailure_WhenSuccess_ReturnsFalse()
    {
        Result<int> result = 42;

        result.IsFailure.ShouldBeFalse();
    }

    [Fact]
    public void IsFailure_WhenFailure_ReturnsTrue()
    {
        Result<int> result = new Error("Test.Error", "Test error");

        result.IsFailure.ShouldBeTrue();
    }

    [Fact]
    public void ImplicitConversion_FromErrorArray_CreatesFailureResult()
    {
        var errors = new[] { new Error("Error.One", "First"), new Error("Error.Two", "Second") };

        Result<int> result = errors;

        result.IsSuccess.ShouldBeFalse();
        result.Errors.Count.ShouldBe(2);
        result.Errors.ShouldContain(errors[0]);
        result.Errors.ShouldContain(errors[1]);
    }

    [Fact]
    public void ToString_WhenSuccess_ReturnsSuccessFormat()
    {
        Result<int> result = 42;

        result.ToString().ShouldBe("Success(42)");
    }

    [Fact]
    public void ToString_WhenFailure_ReturnsFailureFormat()
    {
        Result<int> result = new Error("something went wrong");

        result.ToString().ShouldStartWith("Failure(");
        result.ToString().ShouldContain("something went wrong");
    }

    [Fact]
    public void ToJson_WhenSuccess_ContainsIsSuccessTrueAndValue()
    {
        Result<int> result = 7;

        var json = result.ToJson();

        json.ShouldContain("\"isSuccess\":true");
        json.ShouldContain("\"value\":7");
    }

    [Fact]
    public void ToJson_WhenFailure_ContainsIsSuccessFalseAndErrors()
    {
        Result<int> result = new Error("bad input", "INVALID");

        var json = result.ToJson();

        json.ShouldContain("\"isSuccess\":false");
        json.ShouldContain("\"errors\"");
        json.ShouldContain("bad input");
    }

    [Fact]
    public void Success_NoArg_ReturnsSuccessResultOfUnit()
    {
        var result = Result.Success();

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(Unit.Value);
    }

    [Fact]
    public void Failure_FromIResult_CopiesErrors()
    {
        var original = Result.Failure<string>(new Error("err1"), new Error("err2"));

        var result = Result.Failure<int>(original);

        result.IsSuccess.ShouldBeFalse();
        result.Errors.Count.ShouldBe(2);
    }

    [Fact]
    public void Failure_FromIResult_ThrowsWhenGivenSuccessResult()
    {
        var success = Result.Success("ok");

        Action action = () => Result.Failure<int>(success);

        action.ShouldThrow<ArgumentException>();
    }

    [Fact]
    public void Try_WithCustomErrorConverter_UsesConverter()
    {
        var result = Result.Try<int>(
            () => throw new InvalidOperationException("boom"),
            ex => new Error(ex.Message, "CUSTOM_CODE"));

        result.IsSuccess.ShouldBeFalse();
        result.Errors.Single().Code.ShouldBe("CUSTOM_CODE");
        result.Errors.Single().Message.ShouldBe("boom");
    }

    [Fact]
    public async Task TryAsync_WithCustomErrorConverter_UsesConverter()
    {
        var result = await Result.TryAsync<int>(
            async () =>
            {
                await Task.Delay(1);
                throw new InvalidOperationException("async boom");
            },
            ex => new Error(ex.Message, "ASYNC_CODE"));

        result.IsSuccess.ShouldBeFalse();
        result.Errors.Single().Code.ShouldBe("ASYNC_CODE");
        result.Errors.Single().Message.ShouldBe("async boom");
    }

    [Fact]
    public void Combine_Static_AllSuccess_ReturnsSuccess()
    {
        var r1 = Result.Success(1);
        var r2 = Result.Success("hello");
        var r3 = Result.Success(true);

        var combined = Result.Combine(r1, r2, r3);

        combined.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public void Combine_Static_WithFailures_AggregatesAllErrors()
    {
        var r1 = Result.Success(1);
        var r2 = Result.Failure<string>(new Error("err1"));
        var r3 = Result.Failure<bool>(new Error("err2"));

        var combined = Result.Combine(r1, r2, r3);

        combined.IsSuccess.ShouldBeFalse();
        combined.Errors.Count.ShouldBe(2);
    }

    private class TestUserDto
    {
        public required string Email { get; set; }

        public required string FullName { get; set; }
    }
}
