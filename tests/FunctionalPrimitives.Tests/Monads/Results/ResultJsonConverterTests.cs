using System.Text.Json;
using FunctionalPrimitives.Errors;
using FunctionalPrimitives.Monads.Results;
using Shouldly;
using Xunit;

namespace FunctionalPrimitives.Tests.Monads.Results;

public class ResultJsonConverterTests
{
    private readonly ResultJsonConverterFactory _factory = new();

    // --- ResultJsonConverterFactory ---
    [Fact]
    public void CanConvert_ResultOfInt_ReturnsTrue()
    {
        _factory.CanConvert(typeof(Result<int>)).ShouldBeTrue();
    }

    [Fact]
    public void CanConvert_ResultOfString_ReturnsTrue()
    {
        _factory.CanConvert(typeof(Result<string>)).ShouldBeTrue();
    }

    [Fact]
    public void CanConvert_String_ReturnsFalse()
    {
        _factory.CanConvert(typeof(string)).ShouldBeFalse();
    }

    [Fact]
    public void CanConvert_Int_ReturnsFalse()
    {
        _factory.CanConvert(typeof(int)).ShouldBeFalse();
    }

    [Fact]
    public void CanConvert_PlainObject_ReturnsFalse()
    {
        _factory.CanConvert(typeof(object)).ShouldBeFalse();
    }

    [Fact]
    public void CreateConverter_ReturnsResultJsonConverterOfCorrectType()
    {
        var converter = _factory.CreateConverter(typeof(Result<int>), JsonSerializerOptions.Default);

        converter.ShouldNotBeNull();
        converter.ShouldBeOfType<ResultJsonConverter<int>>();
    }

    [Fact]
    public void CreateConverter_ForDifferentValueTypes_ReturnsCorrectConverterType()
    {
        var stringConverter = _factory.CreateConverter(typeof(Result<string>), JsonSerializerOptions.Default);
        var boolConverter = _factory.CreateConverter(typeof(Result<bool>), JsonSerializerOptions.Default);

        stringConverter.ShouldBeOfType<ResultJsonConverter<string>>();
        boolConverter.ShouldBeOfType<ResultJsonConverter<bool>>();
    }

    // --- ResultJsonConverter<T>.Write ---
    [Fact]
    public void Write_SuccessResult_SerializesIsSuccessTrueAndValue()
    {
        Result<int> result = 42;

        var json = JsonSerializer.Serialize(result);

        json.ShouldContain("\"isSuccess\":true");
        json.ShouldContain("\"value\":42");
        json.ShouldNotContain("\"errors\"");
    }

    [Fact]
    public void Write_FailureResult_SerializesIsSuccessFalseAndErrors()
    {
        Result<int> result = new Error("bad input", "INVALID");

        var json = JsonSerializer.Serialize(result);

        json.ShouldContain("\"isSuccess\":false");
        json.ShouldContain("\"errors\"");
        json.ShouldContain("bad input");
        json.ShouldNotContain("\"value\"");
    }

    [Fact]
    public void Write_FailureWithMultipleErrors_IncludesAllErrors()
    {
        var result = Result.Failure<string>(new Error("first"), new Error("second"));

        var json = JsonSerializer.Serialize(result);

        json.ShouldContain("first");
        json.ShouldContain("second");
    }

    [Fact]
    public void Write_SuccessWithStringValue_SerializesValue()
    {
        Result<string> result = "hello";

        var json = JsonSerializer.Serialize(result);

        json.ShouldContain("\"value\":\"hello\"");
    }

    // --- ResultJsonConverter<T>.Read ---
    [Fact]
    public void Read_AlwaysThrowsNotImplementedException()
    {
        var json = "{\"isSuccess\":true,\"value\":1}";

        Action act = () => JsonSerializer.Deserialize<Result<int>>(json);

        act.ShouldThrow<NotImplementedException>();
    }
}
