using Adria.Application.Contracts;
using Adria.Application.Users;
using Adria.Domain.Shared;
using Adria.Domain.Users;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using UnitTests.Mocks;

namespace UnitTests.Adria.Application;

public class LoginTests
{
    [Fact]
    public async Task LoginWithNonExistingUserThrows()
    {
        ILogger<Login> logger = new NullLogger<Login>();
        IJwtProvider jwtProvider = new MockJwtProvider();
        MockAdoUserRepository repository = new MockAdoUserRepository();
        Login useCase = new(repository, logger, jwtProvider);

        await Assert.ThrowsAsync<UserNotFoundException>(() => useCase.Execute(Guid.NewGuid()));
    }
    
    [Fact]
    public async Task LoginWithExistingUserReturnsJwt()
    {
        ILogger<Login> logger = new NullLogger<Login>();
        IJwtProvider jwtProvider = new MockJwtProvider();
        MockAdoUserRepository repository = new MockAdoUserRepository();
        Login useCase = new(repository, logger, jwtProvider);
        User user = new("DitIsEenUsername");
        await repository.Save(user);
        Assert.NotNull(useCase.Execute(user.Id));
    }
}