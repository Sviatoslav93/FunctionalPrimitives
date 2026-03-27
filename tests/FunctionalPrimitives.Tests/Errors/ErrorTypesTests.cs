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
        var errorWithCode = new ConflictError("Conflict with code", "CONFLICT_CODE");

        // Assert
        error.Type.ShouldBe("conflict");
        error.Message.ShouldBe("Conflict occurred");

        errorWithCode.Type.ShouldBe("conflict");
        errorWithCode.Message.ShouldBe("Conflict with code");
        errorWithCode.Code.ShouldBe("CONFLICT_CODE");
    }

    [Fact]
    public void Should_CreateNotFoundError()
    {
        // Arrange
        var error = new NotFoundError("Not found");
        var errorWithCode = new NotFoundError("Not found with code", "NOT_FOUND_CODE");

        // Assert
        error.Type.ShouldBe("not-found");
        error.Message.ShouldBe("Not found");

        errorWithCode.Type.ShouldBe("not-found");
        errorWithCode.Message.ShouldBe("Not found with code");
        errorWithCode.Code.ShouldBe("NOT_FOUND_CODE");
    }

    [Fact]
    public void Should_CreateValidationError()
    {
        // Arrange
        var error = new ValidationError("Validation failed");
        var errorWithCode = new ValidationError("Validation failed with code", "VALIDATION_CODE");

        // Assert
        error.Type.ShouldBe("validation");
        error.Message.ShouldBe("Validation failed");

        errorWithCode.Type.ShouldBe("validation");
        errorWithCode.Message.ShouldBe("Validation failed with code");
        errorWithCode.Code.ShouldBe("VALIDATION_CODE");
    }

    [Fact]
    public void Should_CreateUnexpectedError()
    {
        // Arrange
        var error = new UnexpectedError("Unexpected error");
        var errorWithCode = new UnexpectedError("Unexpected error with code", "UNEXPECTED_CODE");

        // Assert
        error.Type.ShouldBe("unexpected");
        error.Message.ShouldBe("Unexpected error");

        errorWithCode.Type.ShouldBe("unexpected");
        errorWithCode.Message.ShouldBe("Unexpected error with code");
        errorWithCode.Code.ShouldBe("UNEXPECTED_CODE");
    }

    [Fact]
    public void Should_CreateUnauthorizedError()
    {
        // Arrange
        var error = new UnauthorizedError("Unauthorized");
        var errorWithCode = new UnauthorizedError("Unauthorized with code", "UNAUTHORIZED_CODE");

        // Assert
        error.Type.ShouldBe("unauthorized");
        error.Message.ShouldBe("Unauthorized");

        errorWithCode.Type.ShouldBe("unauthorized");
        errorWithCode.Message.ShouldBe("Unauthorized with code");
        errorWithCode.Code.ShouldBe("UNAUTHORIZED_CODE");
    }

    [Fact]
    public void Should_CreateForbiddenError()
    {
        // Arrange
        var error = new ForbiddenError("Forbidden");
        var errorWithCode = new ForbiddenError("Forbidden with code", "FORBIDDEN_CODE");

        // Assert
        error.Type.ShouldBe("forbidden");
        error.Message.ShouldBe("Forbidden");

        errorWithCode.Type.ShouldBe("forbidden");
        errorWithCode.Message.ShouldBe("Forbidden with code");
        errorWithCode.Code.ShouldBe("FORBIDDEN_CODE");
    }

    [Fact]
    public void Should_CreateTimeoutError()
    {
        // Arrange
        var error = new TimeoutError("Timeout occurred");
        var errorWithCode = new TimeoutError("Timeout with code", "TIMEOUT_CODE");

        // Assert
        error.Type.ShouldBe("timeout");
        error.Message.ShouldBe("Timeout occurred");

        errorWithCode.Type.ShouldBe("timeout");
        errorWithCode.Message.ShouldBe("Timeout with code");
        errorWithCode.Code.ShouldBe("TIMEOUT_CODE");
    }

    [Fact]
    public void Should_CreateInvalidStateError()
    {
        // Arrange
        var error = new InvalidStateError("Invalid state");
        var errorWithCode = new InvalidStateError("Invalid state with code", "INVALID_STATE_CODE");

        // Assert
        error.Type.ShouldBe("invalid-state");
        error.Message.ShouldBe("Invalid state");

        errorWithCode.Type.ShouldBe("invalid-state");
        errorWithCode.Message.ShouldBe("Invalid state with code");
        errorWithCode.Code.ShouldBe("INVALID_STATE_CODE");
    }
}
