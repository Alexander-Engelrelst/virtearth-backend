using Adria.Application.Users;
using Adria.Domain.Shared.Exceptions;
using Adria.Domain.Users;
using Microsoft.Extensions.Logging.Abstractions;
using UnitTests.Mocks;

namespace UnitTests.Adria.Application;

public class GetUserTests
{
    [Fact]
    public async Task NonExistingUserThrows()
    {
        Guid id =  Guid.NewGuid();
        var usecase = new GetUser(new MockAdoUserRepository(), new NullLogger<GetUser>());
        await Assert.ThrowsAsync<UserNotFoundException>(() => usecase.Execute(id));
    }

    [Fact]
    public async Task ExistingUserWorks()
    {
        Guid id =  Guid.NewGuid();
        User user = new("coolusername", id);
        
        var repository = new MockAdoUserRepository();
        await repository.Save(user);
        
        var usecase = new GetUser(repository, new NullLogger<GetUser>());
        Assert.Equal(user, await usecase.Execute(id));
    }
} 