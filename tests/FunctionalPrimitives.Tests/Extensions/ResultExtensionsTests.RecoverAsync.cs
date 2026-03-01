using FunctionalPrimitives.Extensions;
using FunctionalPrimitives.Extensions.Result;
using Shouldly;
using Xunit;

namespace FunctionalPrimitives.Tests.Extensions
{
    public partial class ResultExtensionsTests
    {
        // RecoverAsync<T>(this FunctionalPrimitives<T> result, Func<IEnumerable<Error>, Task<T>> recovery)
        [Fact]
        public async Task RecoverAsync_WithAsyncRecoveryFunc_ShouldReturnOriginalValue_WhenResultIsSuccess()
        {
            // Arrange
            var originalValue = 42;
            var result = Result.Success(originalValue);
            var factoryCalled = false;

            // Act
            var recovered = await result.RecoverAsync(async errors =>
            {
                factoryCalled = true;
                await Task.Delay(1);
                return 0;
            });

            // Assert
            recovered.IsSuccess.ShouldBeTrue();
            recovered.Value.ShouldBe(originalValue);
            factoryCalled.ShouldBeFalse();
        }

        [Fact]
        public async Task RecoverAsync_WithAsyncRecoveryFunc_ShouldInvokeRecoveryAndReturnValue_WhenResultIsFailure()
        {
            // Arrange
            var error = new Error("Something went wrong", "ERR_500");
            var result = Result.Failure<int>(error);
            IEnumerable<Error>? capturedErrors = null;

            // Act
            var recovered = await result.RecoverAsync(async errors =>
            {
                capturedErrors = errors.ToList();
                await Task.Delay(1);
                return 99;
            });

            // Assert
            recovered.IsSuccess.ShouldBeTrue();
            recovered.Value.ShouldBe(99);
            capturedErrors.ShouldNotBeNull();
            capturedErrors.Count().ShouldBe(1);
            capturedErrors.First().ShouldBe(error);
        }

        [Fact]
        public async Task RecoverAsync_WithAsyncRecoveryFunc_ShouldProvideMultipleErrors()
        {
            // Arrange
            var error1 = new Error("Error 1", "CODE1");
            var error2 = new Error("Error 2", "CODE2");
            var error3 = new Error("Error 3", "CODE3");
            var result = Result.Failure<string>(error1, error2, error3);

            // Act
            var recovered = await result.RecoverAsync(async errors =>
            {
                await Task.Delay(1);
                var errorList = errors.ToList();
                return $"Recovered from {errorList.Count} errors";
            });

            // Assert
            recovered.IsSuccess.ShouldBeTrue();
            recovered.Value.ShouldBe("Recovered from 3 errors");
        }

        // RecoverAsync<T>(this FunctionalPrimitives<T> result, Func<IEnumerable<Error>, Task<FunctionalPrimitives<T>>> recovery)
        [Fact]
        public async Task RecoverAsync_WithAsyncResultRecoveryFunc_ShouldReturnOriginalValue_WhenResultIsSuccess()
        {
            // Arrange
            var originalValue = "success";
            var result = Result.Success(originalValue);
            var factoryCalled = false;

            // Act
            var recovered = await result.RecoverAsync(async errors =>
            {
                factoryCalled = true;
                await Task.Delay(1);
                return Result.Success("fallback");
            });

            // Assert
            recovered.IsSuccess.ShouldBeTrue();
            recovered.Value.ShouldBe(originalValue);
            factoryCalled.ShouldBeFalse();
        }

        [Fact]
        public async Task RecoverAsync_WithAsyncResultRecoveryFunc_ShouldInvokeRecoveryAndReturnResult_WhenResultIsFailure()
        {
            // Arrange
            var error = new Error("Database error");
            var result = Result.Failure<string>(error);

            // Act
            var recovered = await result.RecoverAsync(async errors =>
            {
                await Task.Delay(1);
                return Result.Success("recovered value");
            });

            // Assert
            recovered.IsSuccess.ShouldBeTrue();
            recovered.Value.ShouldBe("recovered value");
        }

        [Fact]
        public async Task RecoverAsync_WithAsyncResultRecoveryFunc_ShouldReturnFailure_WhenRecoveryFails()
        {
            // Arrange
            var originalError = new Error("Original error");
            var recoveryError = new Error("Recovery failed");
            var result = Result.Failure<int>(originalError);

            // Act
            var recovered = await result.RecoverAsync(async errors =>
            {
                await Task.Delay(1);
                return Result.Failure<int>(recoveryError);
            });

            // Assert
            recovered.IsSuccess.ShouldBeFalse();
            recovered.Errors.Count().ShouldBe(1);
            recovered.Errors.First().ShouldBe(recoveryError);
        }

        // RecoverAsync<T>(this Task<FunctionalPrimitives<T>> task, Func<IEnumerable<Error>, Task<FunctionalPrimitives<T>>> recovery)
        [Fact]
        public async Task RecoverAsync_FromTask_ShouldReturnOriginalValue_WhenResultIsSuccess()
        {
            // Arrange
            var originalValue = 123;
            var resultTask = Task.FromResult(Result.Success(originalValue));
            var factoryCalled = false;

            // Act
            var recovered = await resultTask.RecoverAsync(async errors =>
            {
                factoryCalled = true;
                await Task.Delay(1);
                return Result.Success(0);
            });

            // Assert
            recovered.IsSuccess.ShouldBeTrue();
            recovered.Value.ShouldBe(originalValue);
            factoryCalled.ShouldBeFalse();
        }

        [Fact]
        public async Task RecoverAsync_FromTask_ShouldInvokeRecoveryAndReturnResult_WhenResultIsFailure()
        {
            // Arrange
            var error = new Error("Async operation failed");
            var resultTask = Task.FromResult(Result.Failure<double>(error));

            // Act
            var recovered = await resultTask.RecoverAsync(async errors =>
            {
                await Task.Delay(1);
                return Result.Success(3.14);
            });

            // Assert
            recovered.IsSuccess.ShouldBeTrue();
            recovered.Value.ShouldBe(3.14);
        }

        [Fact]
        public async Task RecoverAsync_FromTask_ShouldHandleAsyncTask()
        {
            // Arrange
            async Task<Result<string>> GetFailedResultAsync()
            {
                await Task.Delay(1);
                return Result.Failure<string>(new Error("Delayed error"));
            }

            // Act
            var recovered = await GetFailedResultAsync().RecoverAsync(async errors =>
            {
                await Task.Delay(1);
                return Result.Success("async recovery");
            });

            // Assert
            recovered.IsSuccess.ShouldBeTrue();
            recovered.Value.ShouldBe("async recovery");
        }

        // RecoverAsync<T>(this Task<FunctionalPrimitives<T>> task, T fallback)
        [Fact]
        public async Task RecoverAsync_FromTaskWithFallback_ShouldReturnOriginalValue_WhenResultIsSuccess()
        {
            // Arrange
            var originalValue = "original";
            var fallbackValue = "fallback";
            var resultTask = Task.FromResult(Result.Success(originalValue));

            // Act
            var recovered = await resultTask.RecoverAsync(fallbackValue);

            // Assert
            recovered.IsSuccess.ShouldBeTrue();
            recovered.Value.ShouldBe(originalValue);
        }

        [Fact]
        public async Task RecoverAsync_FromTaskWithFallback_ShouldReturnFallbackValue_WhenResultIsFailure()
        {
            // Arrange
            var fallbackValue = 777;
            var resultTask = Task.FromResult(Result.Failure<int>(new Error("Task failed")));

            // Act
            var recovered = await resultTask.RecoverAsync(fallbackValue);

            // Assert
            recovered.IsSuccess.ShouldBeTrue();
            recovered.Value.ShouldBe(fallbackValue);
        }

        [Fact]
        public async Task RecoverAsync_FromTaskWithFallback_ShouldHandleAsyncTask()
        {
            // Arrange
            async Task<Result<string>> GetFailedResultAsync()
            {
                await Task.Delay(1);
                return Result.Failure<string>(new Error("Async error"));
            }

            // Act
            var recovered = await GetFailedResultAsync().RecoverAsync("recovered");

            // Assert
            recovered.IsSuccess.ShouldBeTrue();
            recovered.Value.ShouldBe("recovered");
        }

        [Fact]
        public async Task RecoverAsync_FromTaskWithFallback_ShouldHandleNullFallback()
        {
            // Arrange
            var resultTask = Task.FromResult(Result.Failure<string?>(new Error("Error")));

            // Act
            var recovered = await resultTask.RecoverAsync((string?)null);

            // Assert
            recovered.IsSuccess.ShouldBeTrue();
            recovered.Value.ShouldBeNull();
        }

        [Fact]
        public async Task RecoverAsync_WithAsyncRecoveryFunc_ShouldHandleNullReturnValue()
        {
            // Arrange
            var result = Result.Failure<string?>(new Error("Error"));

            // Act
            var recovered = await result.RecoverAsync(async errors =>
            {
                await Task.Delay(1);
                return (string?)null;
            });

            // Assert
            recovered.IsSuccess.ShouldBeTrue();
            recovered.Value.ShouldBeNull();
        }
    }
}
