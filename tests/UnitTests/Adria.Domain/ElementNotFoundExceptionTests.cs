using Adria.Domain.Shared.Exceptions;

namespace UnitTests.Adria.Domain;

public class ElementNotFoundExceptionTests
{
    [Fact]
    public void Constructor_WithMessage_CreatesException()
    {
        // Arrange
        var message = "Element not found";
        
        // Act
        var exception = new ElementNotFoundException(message);
        
        // Assert
        Assert.Equal(message, exception.Message);
        Assert.Null(exception.InnerException);
    }

    [Fact]
    public void Constructor_WithMessageAndInnerException_CreatesException()
    {
        // Arrange
        var message = "Element not found";
        var innerException = new Exception("Inner exception");
        
        // Act
        var exception = new ElementNotFoundException(message, innerException);
        
        // Assert
        Assert.Equal(message, exception.Message);
        Assert.Equal(innerException, exception.InnerException);
    }

    [Fact]
    public void ForId_WithGenericType_CreatesExceptionWithCorrectMessage()
    {
        // Arrange
        var id = Guid.NewGuid();
        
        // Act
        var exception = ElementNotFoundException.ForId<User>(id);
        
        // Assert
        Assert.Contains("Element of type User", exception.Message);
        Assert.Contains(id.ToString(), exception.Message);
        Assert.Contains("not found", exception.Message);
    }

    [Fact]
    public void ForId_WithDifferentGenericType_CreatesExceptionWithCorrectMessage()
    {
        // Arrange
        var id = Guid.NewGuid();
        
        // Act
        var exception = ElementNotFoundException.ForId<TodoList>(id);
        
        // Assert
        Assert.Contains("Element of type TodoList", exception.Message);
        Assert.Contains(id.ToString(), exception.Message);
        Assert.Contains("not found", exception.Message);
    }

    // Helper classes for generic type testing
    private class User { }
    private class TodoList { }
}