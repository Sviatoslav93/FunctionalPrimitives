using FunctionalPrimitives.Errors;
using Shouldly;
using Xunit;

namespace FunctionalPrimitives.Tests.Errors;

public class ErrorTypesTests
{
    [Fact]
    public void Should_CreateConflictError()
    {
        // Arrange
        var error = new ConflictError("Conflict occurred");

        // Assert
        error.Type.ShouldBe("conflict");
    }

    [Fact]
    public void Should_CreateNotFoundError()
    {
        // Arrange
        var error = new NotFoundError("Not found");

        // Assert
        error.Type.ShouldBe("not-found");
    }

    [Fact]
    public void Should_CreateValidationError()
    {
        // Arrange
        var error = new ValidationError("Validation failed");

        // Assert
        error.Type.ShouldBe("validation");
    }

    [Fact]
    public void Should_CreateUnexpectedError()
    {
        // Arrange
        var error = new UnexpectedError("Unexpected error");

        // Assert
        error.Type.ShouldBe("unexpected");
    }

    [Fact]
    public void Should_CreateUnauthorizedError()
    {
        // Arrange
        var error = new UnauthorizedError("Unauthorized");

        // Assert
        error.Type.ShouldBe("unauthorized");
    }

    [Fact]
    public void Should_CreateForbiddenError()
    {
        // Arrange
        var error = new ForbiddenError("Forbidden");

        // Assert
        error.Type.ShouldBe("forbidden");
    }

    [Fact]
    public void Should_CreateTimeoutError()
    {
        // Arrange
        var error = new TimeoutError("Timeout occurred");

        // Assert
        error.Type.ShouldBe("timeout");
    }

    [Fact]
    public void Should_CreateInvalidStateError()
    {
        // Arrange
        var error = new InvalidStateError("Invalid state");

        // Assert
        error.Type.ShouldBe("invalid-state");
    }
}
