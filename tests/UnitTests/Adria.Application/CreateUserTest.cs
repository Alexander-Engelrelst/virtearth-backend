using Adria.Application.Users;
using Adria.Domain.Shared.Exceptions;
using Microsoft.Extensions.Logging.Abstractions;
using UnitTests.Mocks;

namespace UnitTests.Adria.Application;

public class CreateUserTest
{
    [Fact]
    public async Task ExecuteUsernameSearchWithExistingUserThrows()
    {
        var username = "jeffken";
        var MockQuery = new MockUserExistsQuery();
        var useCase = new CheckUsernameInUse(MockQuery, new NullLogger<CheckUsernameInUse>());
        var input = new CheckUsernameInUseInput(username);
        await Assert.ThrowsAsync<UsernameAlreadyExistsException>(() => useCase.Execute(input));
    }
    
    [Fact]
    public async Task ExecuteUsernameSearchWithNonExistingUserReturnsFalse()
    {
        var username = "bobke";
        var MockQuery = new MockUserExistsQuery();
        var useCase = new CheckUsernameInUse(MockQuery, new NullLogger<CheckUsernameInUse>());
        var input = new CheckUsernameInUseInput(username);
        Assert.False(await useCase.Execute(input));
    }
}