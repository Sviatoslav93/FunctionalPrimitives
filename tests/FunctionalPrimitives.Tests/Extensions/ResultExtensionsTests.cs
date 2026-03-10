using FunctionalPrimitives.Extensions.Result;
using Xunit;

namespace FunctionalPrimitives.Tests.Extensions;

public partial class ResultExtensionsTests
{
    [Fact]
    public void ToResult_ShouldWrapValueInResult()
    {
        // Arrange
        var value = 42;

        // Act
        var result = value.ToResult();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(value, result.Value);
    }
}
