using FunctionalPrimitives.Errors;
using FunctionalPrimitives.Errors.Extensions;
using Shouldly;
using Xunit;

namespace FunctionalPrimitives.Tests.Errors;

public class ErrorExtensionsTests
{
    [Fact]
    public void Should_ExtendError()
    {
        // Arrange
        var error = new Error("Validation failed");

        // Act
        var extendedError = error
            .WithMetadata("key", "value")
            .WithMetadata("anotherKey", 123);

        // Assert
        extendedError.Metadata["key"].ShouldBe("value");
        extendedError.Metadata["anotherKey"].ShouldBe(123);
    }
}
