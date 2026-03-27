using FunctionalPrimitives.Errors;
using FunctionalPrimitives.Monads.Options;
using FunctionalPrimitives.Monads.Options.Extensions;
using Shouldly;
using Xunit;

namespace FunctionalPrimitives.Tests.Monads.Options.Extensions;

public partial class OptionExtensionsTests
{
    [Fact]
    public void ToMaybe_ReferenceType_ShouldReturnSome_WhenNotNull()
    {
        string? value = "hello";

        var option = value.ToMaybe();

        option.HasValue.ShouldBeTrue();
        option.Value.ShouldBe("hello");
    }

    [Fact]
    public void ToMaybe_ReferenceType_ShouldReturnNone_WhenNull()
    {
        string? value = null;

        var option = value.ToMaybe();

        option.HasValue.ShouldBeFalse();
    }

    [Fact]
    public void ToMaybe_ValueType_ShouldReturnSome_WhenHasValue()
    {
        int? value = 42;

        var option = value.ToMaybe();

        option.HasValue.ShouldBeTrue();
        option.Value.ShouldBe(42);
    }

    [Fact]
    public void ToMaybe_ValueType_ShouldReturnNone_WhenNull()
    {
        int? value = null;

        var option = value.ToMaybe();

        option.HasValue.ShouldBeFalse();
    }

    [Fact]
    public void ToResult_ShouldReturnSuccess_WhenSome()
    {
        var option = Option<int>.Some(7);
        var error = new Error("none");

        var result = option.ToResult(error);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(7);
    }

    [Fact]
    public void ToResult_ShouldReturnFailure_WhenNone()
    {
        var option = None<int>();
        var error = new Error("none");

        var result = option.ToResult(error);

        result.IsFailure.ShouldBeTrue();
        result.Errors.ShouldContain(error);
    }

    [Fact]
    public void GetValueOr_ShouldReturnFallback_WhenNone()
    {
        var option = None<int>();

        var value = option.GetValueOr(99);

        value.ShouldBe(99);
    }

    [Fact]
    public async Task ToResultAsync_ShouldReturnFailure_WhenNone()
    {
        var optionTask = Task.FromResult(None<int>());
        var error = new Error("missing");

        var result = await optionTask.ToResultAsync(error);

        result.IsFailure.ShouldBeTrue();
        result.Errors.ShouldContain(error);
    }
}
