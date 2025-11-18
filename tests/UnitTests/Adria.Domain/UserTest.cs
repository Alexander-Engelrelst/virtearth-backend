using Adria.Domain.Shared.Exceptions;
using Adria.Domain.Users;

namespace UnitTests.Adria.Domain;

public class UserTest
{
    [Fact]
    public void UserWithToShortNameThrows()
    {
        Assert.Throws<InvalidUsernameException>(() => new User("ab"));
    }

    [Fact]
    public void UserWithToLongNameThrows()
    {
        Assert.Throws<InvalidUsernameException>(
            () => new User(string.Concat(Enumerable.Repeat("a", 100)))
            );
    }

    [Fact]
    public void EmptyUserNameThrows()
    {
        Assert.Throws<InvalidUsernameException>(() => new User(""));
    }
    
    [Fact]
    public void WhiteSpaceUserNameThrows()
    {
        Assert.Throws<InvalidUsernameException>(() => new User("     "));
    }
    
    [Fact]
    public void InvalidCharactersThrows()
    {
        Assert.Throws<InvalidUsernameException>(() => new User("test#"));
    }

    [Fact]
    public void ValidUserName()
    {
        User user = new("valid._-");
        Assert.NotNull(user);
    }
    
}