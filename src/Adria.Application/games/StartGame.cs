using Adria.Application.Contracts;
using Adria.Domain.games;
using Microsoft.Extensions.Logging;

namespace Adria.Application.games;

public sealed record StartGameInput(Guid GameId, Guid UserId);

//TODO add the correct addscoped for this when the class is finished
public sealed class StartGame : IUseCase<StartGameInput, Task<Game>>
{
    private readonly IGameTypeQuery _gameTypeQuery;
    private readonly ILogger<StartGame> _logger;

    public StartGame(
        IGameTypeQuery gameTypeQuery,
        ILogger<StartGame> logger
    )
    {
        _gameTypeQuery = gameTypeQuery;
        _logger = logger;
    }
    
    public async Task<Game> Execute(StartGameInput input)
    {
        GameTypes gameType = await _gameTypeQuery.Fetch(input.GameId);
        throw new NotImplementedException();
        //todo finish this lol
    }
}