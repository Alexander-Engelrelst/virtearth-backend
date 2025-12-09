using Adria.Domain.Shared.Exceptions;

namespace UnitTests.Adria.Domain.exceptions;

public class InvalidUsernameExceptionTests
{
    [Fact]
    public void Constructor_WithMessage_CreatesException()
    {
        // Arrange
        var username = "jeffrey_;";
        
        // Act
        var exception = new InvalidUsernameException(username);

        // Assert
        Assert.Equal(
            $"Invalid username {username}: must be between 3 and 40 characters long and may only contain a-zA-Z0-9 and/or -_. character.",
            exception.Message
        );
        Assert.Null(exception.InnerException);
    }

    [Fact]
    public void Constructor_WithMessageAndInnerException_CreatesException()
    {
        // Arrange
        var username = "jeffrey_;";
        var innerException = new Exception("Inner exception");
        
        // Act
        var exception = new InvalidUsernameException(username, innerException);
        
        // Assert
        Assert.Equal(
            $"Invalid username {username}: must be between 3 and 40 characters long and may only contain a-zA-Z0-9 and/or -_. character.",
            exception.Message
        );
        Assert.Equal(innerException, exception.InnerException);
    }
}