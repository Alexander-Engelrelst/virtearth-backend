using Adria.Application.Users;
using Adria.Domain.Shared;
using Microsoft.Extensions.Logging.Abstractions;
using UnitTests.Mocks;

namespace UnitTests.Adria.Application;

public class CheckUsernameInUseTests
{
    [Fact]
    public async Task InvalidUsernameThrows()
    {
        var usecase = new CheckUsernameInUse(
            new MockUserExistsQuery(),
            new NullLogger<CheckUsernameInUse>()
        );

        await Assert.ThrowsAsync<InvalidUsernameException>(() =>
            usecase.Execute(new CheckUsernameInUseInput("this is not valid"))
        );
    }
    
    [Fact]
    public async Task ExistingUsernameThrows()
    {
        var usecase = new CheckUsernameInUse(
            new MockUserExistsQuery(),
            new NullLogger<CheckUsernameInUse>()
        );
        await Assert.ThrowsAsync<UsernameAlreadyExistsException>(() =>
            usecase.Execute(new CheckUsernameInUseInput(MockUserExistsQuery._existingUserName))
        );
    }
    
    [Fact]
    public async Task NonExistingUsernameReturnsFalse()
    {
        var usecase = new CheckUsernameInUse(
            new MockUserExistsQuery(),
            new NullLogger<CheckUsernameInUse>()
        );
        Assert.False(await usecase.Execute(new CheckUsernameInUseInput("thisusernamesuredoesnotexist")));
    }
}