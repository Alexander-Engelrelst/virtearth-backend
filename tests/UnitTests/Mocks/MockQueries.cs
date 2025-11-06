using Adria.Application.Contracts;
using Adria.Domain.Shared.Exceptions;

namespace UnitTests.Mocks;

public class MockUserExistsQuery : IUserExistsQuery
{
    private readonly string _existingUserName = "jeffken";
    public Task<bool> Fetch(string username)
    {
        if (username == _existingUserName)
        {
            throw new UsernameAlreadyExistsException(username);
        }
        else {
            return Task.FromResult(false);
        }
    }
}