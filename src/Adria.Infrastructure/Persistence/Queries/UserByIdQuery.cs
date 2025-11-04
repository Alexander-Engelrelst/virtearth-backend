using Adria.Application.Contracts;
using Adria.Domain.Users;

namespace Adria.Infrastructure.Persistence.Queries;

public sealed class UserByIdQuery : IUserByIdQuery
{
    public Task<User> Fetch(Guid id)
    {
        throw new NotImplementedException();
    }
}