using Adria.Application.Contracts.Data;
using Adria.Application.Users;
using Adria.Domain.Shared.Exceptions;
using Adria.Domain.Users;
using Microsoft.Extensions.Logging.Abstractions;
using UnitTests.Mocks;

namespace UnitTests.Adria.Application;

public class ChangeUsernameTest
{
    [Fact]
    public async Task ChangeUsernameToAlreadyExistingThrows()
    {
        var mockRepository = new MockAdoUserRepository();
        
        var usecase = new ChangeUserName(
            mockRepository,
            new NullLogger<ChangeUserName>(),
            new MockJwtProvider(),
            new MockUserExistsQuery()
        );
        Guid id  = Guid.NewGuid();
        await mockRepository.Save(new User("jeff", id));
        
        var input = new ChangeUserNameInput(new User("kaas", Guid.NewGuid()), MockUserExistsQuery._existingUserName);
        await Assert.ThrowsAsync<UsernameAlreadyExistsException>(() => usecase.Execute(input));
    } 
    
    [Fact]
    public async Task ChangeUsernameToInvalidThrows()
    {
        var mockRepository = new MockAdoUserRepository();
        
        var usecase = new ChangeUserName(
            mockRepository,
            new NullLogger<ChangeUserName>(),
            new MockJwtProvider(),
            new MockUserExistsQuery()
        );
        Guid id  = Guid.NewGuid();
        await mockRepository.Save(new User("jeff", id));
        
        var input = new ChangeUserNameInput(new User("kaas", Guid.NewGuid()), "thisIsNotValid{]");
        await Assert.ThrowsAsync<InvalidUsernameException>(() => usecase.Execute(input));
    } 
    
    [Fact]
    public async Task ChangeUsernameReturnsJwt()
    {
        var mockRepository = new MockAdoUserRepository();
        
        var usecase = new ChangeUserName(
            mockRepository,
            new NullLogger<ChangeUserName>(),
            new MockJwtProvider(),
            new MockUserExistsQuery()
        );
        Guid id  = Guid.NewGuid();
        User user = new User("jeff", id);
        await mockRepository.Save(user);
        
        var input = new ChangeUserNameInput(new User("kaas", Guid.NewGuid()), "validname");
        UserData data = await usecase.Execute(input);
        Assert.NotNull(data);
        Assert.Equal("validname", data.User.Username);
        Assert.Equal(MockJwtProvider._jwtToken, data.JwtToken);
    } 
}