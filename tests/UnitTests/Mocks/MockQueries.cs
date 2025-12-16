using Adria.Application.Contracts;
using Adria.Domain.Shared;

namespace UnitTests.Mocks;

public class MockUserExistsQuery : IUserExistsQuery
{
    public static readonly string _existingUserName = "jeffken";
    public Task<bool> Fetch(string username)
    {
        if (username == _existingUserName)
        {
            throw new UsernameAlreadyExistsException(username);
        }

        return Task.FromResult(false);
    }
}