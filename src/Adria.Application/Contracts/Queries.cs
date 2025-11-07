using Adria.Domain.games;
using Adria.Domain.Users;

namespace Adria.Application.Contracts;

public interface IUserExistsQuery
{
    Task<bool> Fetch(string username);
}

public interface IGameLocationsQuery
{
    Task<List<GameLocation>> Fetch();
}