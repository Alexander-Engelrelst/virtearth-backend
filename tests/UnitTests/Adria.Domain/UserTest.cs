using Adria.Domain.Shared.Exceptions;
using Adria.Domain.Users;

namespace UnitTests.Adria.Domain;

public class UserTest
{
    [Fact]
    public void UserWithToShortNameThrows()
    {
        Assert.Throws<InvalidUsernameException>(() => new User("ab", Avatar.AlbertEinstein));
    }

    [Fact]
    public void UserWithToLongNameThrows()
    {
        Assert.Throws<InvalidUsernameException>(
            () => new User(string.Concat(Enumerable.Repeat("a", 100)), Avatar.AlbertEinstein)
            );
    }

    [Fact]
    public void EmptyUserNameThrows()
    {
        Assert.Throws<InvalidUsernameException>(() => new User("", Avatar.AlbertEinstein));
    }
    
    [Fact]
    public void WhiteSpaceUserNameThrows()
    {
        Assert.Throws<InvalidUsernameException>(() => new User("     ", Avatar.AlbertEinstein));
    }
    
    [Fact]
    public void InvalidCharactersThrows()
    {
        Assert.Throws<InvalidUsernameException>(() => new User("test#", Avatar.AlbertEinstein));
    }

    [Fact]
    public void ValidUserName()
    {
        User user = new("valid._-", Avatar.AlbertEinstein);
    }
    
}