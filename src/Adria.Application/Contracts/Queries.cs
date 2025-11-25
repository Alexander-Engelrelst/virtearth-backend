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

public interface IArtifactsQuery
{
    Task<ReadOnlyCollection<MazeArtifact>> Fetch(Guid id);
}