using Adria.Application.Contracts;
using Adria.Domain.games;
using Microsoft.Extensions.Logging;

namespace Adria.Application.games;

public class GetGamesLocations : IUseCase<IReadOnlyCollection<GameLocation>>
{
    private readonly IGameLocationsQuery _gameLocationsQuery;
    private readonly ILogger<GetGamesLocations> _logger;

    public GetGamesLocations(
        IGameLocationsQuery gameLocationsQuery,
        ILogger<GetGamesLocations> logger
    )
    {
        _gameLocationsQuery = gameLocationsQuery;
        _logger = logger;
    }
    
    
    public async Task<IReadOnlyCollection<GameLocation>> Execute()
    {
        return await _gameLocationsQuery.Fetch();
    }
}