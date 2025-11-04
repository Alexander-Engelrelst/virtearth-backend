using Adria.Domain.Users;

namespace Adria.Application.Contracts;

public interface IUserByNameQuery
{
    Task<bool> Fetch(string username);
}