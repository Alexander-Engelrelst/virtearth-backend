using Adria.Domain.Shared;
using Xunit.Abstractions;

namespace UnitTests.Adria.Domain.exceptions;

public class ActiveGameNotFoundExceptionTests
{
    [Fact]
    public void Constructor_WithMessage_CreatesException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        
        // Act
        var exception = new ActiveGameNotFoundException(userId);

        // Assert
        Assert.Equal(
            $"User {userId} is currently not playing a game", exception.Message
        );
        Assert.Null(exception.InnerException);
    }

    [Fact]
    public void Constructor_WithMessageAndInnerException_CreatesException()
    {
        // Arrange
        var innerException = new Exception("Inner exception");
        var userId = Guid.NewGuid();
        
        // Act
        var exception = new ActiveGameNotFoundException(userId, innerException);

        // Assert
        Assert.Equal(
            $"User {userId} is currently not playing a game", exception.Message
        );
        Assert.Equal(innerException, exception.InnerException);
    }
}