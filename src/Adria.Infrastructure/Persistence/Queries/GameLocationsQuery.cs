using Adria.Application.Contracts;
using Adria.Domain.games;

namespace Adria.Infrastructure.Persistence.Queries;

public class GameLocationsQuery : IGameLocationsQuery
{
    public Task<List<GameLocation>> Fetch()
    {
        throw new NotImplementedException();
    }
}