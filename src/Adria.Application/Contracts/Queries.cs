using Adria.Domain.Users;

namespace Adria.Application.Contracts;

public interface IUserExistsQuery
{
    Task<bool> Fetch(string username);
}