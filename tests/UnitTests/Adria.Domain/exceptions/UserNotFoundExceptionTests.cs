using Adria.Domain.Shared;

namespace UnitTests.Adria.Domain.exceptions;

public class UserNotFoundExceptionTests
{
    [Fact]
    public void Constructor_WithMessage_CreatesException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        
        // Act
        var exception = new UserNotFoundException(userId);

        // Assert
        Assert.Equal($"User with id {userId} not found", exception.Message);
        Assert.Null(exception.InnerException);
    }

    [Fact]
    public void Constructor_WithMessageAndInnerException_CreatesException()
    {
        // Arrange
        var innerException = new Exception("Inner exception");
        var userId = Guid.NewGuid();
        
        // Act
        var exception = new UserNotFoundException(userId, innerException);

        // Assert
        Assert.Equal($"User with id {userId} not found", exception.Message);

        Assert.Equal(innerException, exception.InnerException);
    }
}