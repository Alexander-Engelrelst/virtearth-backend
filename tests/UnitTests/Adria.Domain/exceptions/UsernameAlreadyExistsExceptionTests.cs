using Adria.Domain.Shared;

namespace UnitTests.Adria.Domain.exceptions;

public class UsernameAlreadyExistsExceptionTests
{
    [Fact]
    public void Constructor_WithMessage_CreatesException()
    {
        // Arrange
        var username = "jeffrey";
        
        // Act
        var exception = new UsernameAlreadyExistsException(username);

        // Assert
        Assert.Equal($"username {username} already in use.", exception.Message);
        Assert.Null(exception.InnerException);
    }

    [Fact]
    public void Constructor_WithMessageAndInnerException_CreatesException()
    {
        // Arrange
        var username = "jeffrey";
        var innerException = new Exception("Inner exception");
        
        // Act
        var exception = new UsernameAlreadyExistsException(username, innerException);
        
        // Assert
        Assert.Equal($"username {username} already in use.", exception.Message);
        Assert.Equal(innerException, exception.InnerException);
    }
}