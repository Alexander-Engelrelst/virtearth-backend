using Adria.Domain.Shared.Exceptions;
using Adria.Domain.Users;

namespace UnitTests.Mocks;

public class MockAdoUserRepository : IUserRepository
{
    private readonly List<User> _users = [];
    public Task Save(User user)
    {
        ArgumentNullException.ThrowIfNull(user);
        
        if (_users.Any(u => u.Username == user.Username))
        {
            throw new UsernameAlreadyExistsException(user.Username);
        }
        
        _users.Add(user);
        
        return Task.CompletedTask;
    }

    public Task<User?> ById(Guid id)
    {
        return Task.FromResult(_users.FirstOrDefault(u => u.Id == id));
    }
}