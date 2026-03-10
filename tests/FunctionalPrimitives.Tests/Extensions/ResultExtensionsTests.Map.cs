using FunctionalPrimitives.Extensions.Result;
using Shouldly;
using Xunit;

namespace FunctionalPrimitives.Tests.Extensions;

public partial class ResultExtensionsTests
{
    [Fact]
    public void Map_ShouldProjectValue_WhenResultIsSuccess()
    {
        var result = Result.Success(21);

        var actual = result.Map(x => x * 2);

        actual.IsSuccess.ShouldBeTrue();
        actual.Value.ShouldBe(42);
    }

    [Fact]
    public void Map_ShouldPropagateFailure_WhenResultIsFailure()
    {
        var error = new Error("map failed");
        var result = Result.Failure<int>(error);

        var actual = result.Map(x => x * 2);

        actual.IsSuccess.ShouldBeFalse();
        actual.Errors.ShouldContain(error);
    }

    [Fact]
    public void Select_ShouldProjectValue_WhenResultIsSuccess()
    {
        var result = Result.Success(10);

        var actual = result.Select(x => x + 5);

        actual.IsSuccess.ShouldBeTrue();
        actual.Value.ShouldBe(15);
    }

    [Fact]
    public void Map_ShouldThrow_WhenMapperThrows()
    {
        var result = Result.Success(10);
        int ThrowingMapper(int _) => throw new InvalidOperationException("boom");

        var ex = Should.Throw<InvalidOperationException>(() => result.Map(ThrowingMapper));

        ex.Message.ShouldBe("boom");
    }

    [Fact]
    public void QueryExpression_ResultSource_ShouldCompileAndProject_WhenSingleFromClause()
    {
        var source = Result.Success(3);

        var actual = from x in source
                     select x * 4;

        actual.IsSuccess.ShouldBeTrue();
        actual.Value.ShouldBe(12);
    }
}
