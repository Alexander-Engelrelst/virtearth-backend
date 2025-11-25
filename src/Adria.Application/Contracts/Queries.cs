using System.Collections.ObjectModel;
using Adria.Domain.games;

namespace Adria.Application.Contracts;

public interface IUserExistsQuery
{
    Task<bool> Fetch(string username);
}

public interface IGameLocationsQuery
{
    Task<ReadOnlyCollection<GameLocation>> Fetch();
}

public interface IGameTypeQuery
{
    Task<GameTypes> Fetch(Guid id);
}
