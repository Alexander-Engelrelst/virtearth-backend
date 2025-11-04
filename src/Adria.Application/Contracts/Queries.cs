using Adria.Domain.Users;

namespace Adria.Application.Contracts;

public interface IUserByIdQuery
{
    Task<User> Fetch(Guid id);
}