using FluentAssertions;
using Result.Abstractions;
using Xunit;

namespace Result.Tests;

public class ResultTests
{
    [Fact]
    public void Should_CreateFailedResult()
    {
        var result = Result<int>.Failed(new Error("test"));

        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public void Should_CreateSuccessResultForValueType()
    {
        var result = Result<int>.Success(1);

        result.Value.Should().Be(1);
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void Should_CreateSuccessResultForNullableValueType()
    {
        var result = Result<int?>.Success(null);

        result.Value.Should().BeNull();
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void Should_CreateSuccessResultForReferenceType()
    {
        var testDto = new TestUserDto(
            fullName: "John Doe",
            email: "test@gmail.com");
        var result = Result<TestUserDto>.Success(testDto);

        result.Value.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void Should_CreateSuccessResultForNullableReferenceType()
    {
        var testDto = default(TestUserDto);
        var result = Result<TestUserDto?>.Success(testDto);

        result.Value.Should().BeNull();
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void Should_CreateFailedResultWithSeveralErrors()
    {
        var result = Result<int>.Failed(new Error("error one"), new Error("error two"));

        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().HaveCount(2);
    }

    [Fact]
    public void Should_CreateFailedResultWithErrorsList()
    {
        var result = Result<int>.Failed(new List<Error>
        {
            new("error one"),
            new("error two"),
        });

        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().HaveCount(2);
    }

    [Fact]
    public void Should_DeconstructFailedResult()
    {
        var (value, errors) = Result<int>.Failed(new Error("test"));

        value.Should().Be(0);
        errors.Should().HaveCount(1);
    }

    [Fact]
    public void Should_DeconstructSuccessResult()
    {
        var (value, errors) = Result<int>.Success(1);

        value.Should().Be(1);
        errors.Should().BeEmpty();

        var (value2, errors2) = Result<int>.Failed(new Error("test"));
        value2.Should().Be(0);
        errors2.Should().HaveCount(1);
    }

    [Fact]
    public void Should_GetValue_When_ResultIsSuccess()
    {
        var result = Result<int>.Success(1);

        result.Value.Should().Be(1);
    }

    [Fact]
    public void Should_ImplicitConvertErrorToResult()
    {
        Result<int> result = new Error("error");

        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public void Should_ImplicitConvertArrayOfErrorToResult()
    {
        Result<int> result = new[] { new Error("err1"), new Error("err2"), new Error("err3") };

        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public void Should_ImplicitConvertValueToResult()
    {
        Result<int> result = 1;

        result.IsSuccess.Should().BeTrue();
    }

    private class TestUserDto
    {
        public TestUserDto()
        {
        }

        public TestUserDto(string fullName, string email)
        {
            FullName = fullName;
            Email = email;
        }

        public string Email { get; set; } = null!;

        public string FullName { get; set; } = null!;
    }
}
