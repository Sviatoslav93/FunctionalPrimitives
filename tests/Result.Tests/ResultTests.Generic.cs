using Shouldly;
using Xunit;

namespace Result.Tests;

public partial class ResultTests
{
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
        var testDto = new TestUserDto(
            fullName: "John Doe",
            email: "test@gmail.com");
        var result = Result.Success(testDto);

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
    public void Should_CreateSuccessFailedGenericResult()
    {
        var res1 = Result.Failure<int>(new Error("error"));
        var res2 = Result.Failure<string>(new Error("error"));
        var res3 = Result.Failure<int>([new Error("error1"), new Error("error2")]);
        var res4 = Result.Failure<int>(new Error("error1"), new Error("error2"));

        res1.IsSuccess.ShouldBeFalse();
        res2.IsSuccess.ShouldBeFalse();
        res3.IsSuccess.ShouldBeFalse();
        res4.IsSuccess.ShouldBeFalse();
    }

    [Fact]
    public void Should_DeconstructFailedResult()
    {
        var (value, errors) = Result.Failure<int>(new Error("test"));

        value.ShouldBe(0);
        errors.ShouldHaveSingleItem();
    }

    [Fact]
    public void Should_DeconstructSuccessResult()
    {
        var (value, errors) = Result.Success(1);

        value.ShouldBe(1);
        errors.ShouldBeEmpty();

        var (value2, errors2) = Result.Failure<int>(new Error("test"));
        value2.ShouldBe(0);
        errors2.ShouldHaveSingleItem();
    }

    [Fact]
    public void Should_GetValue_When_ResultIsSuccess()
    {
        var result = Result.Success(1);

        result.Value.ShouldBe(1);
    }

    [Fact]
    public void Should_ImplicitConvertErrorToResult()
    {
        Result<int> result = new Error("error");

        result.IsSuccess.ShouldBeFalse();
    }

    [Fact]
    public void Should_ImplicitConvertArrayOfErrorToResult()
    {
        Result<int> result = new[] { new Error("err1"), new Error("err2"), new Error("err3") };

        result.IsSuccess.ShouldBeFalse();
    }

    [Fact]
    public void Should_ImplicitConvertValueToResult()
    {
        Result<int> result = 1;

        result.IsSuccess.ShouldBeTrue();
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
