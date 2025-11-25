using System.Diagnostics;
using Adria.Application.Contracts;
using Adria.Domain.games;
using Microsoft.Extensions.Logging;

namespace Adria.Application.games;

public sealed record StartGameInput(Guid GameId, Guid UserId);

//TODO add the correct addscoped for this when the class is finished
public sealed class StartGame : IUseCase<StartGameInput, Task<Game>>
{
    private readonly ILogger<StartGame> _logger;
    private readonly IGameRepository _gameRepository;
    public StartGame(
        ILogger<StartGame> logger,
        IGameRepository gameRepository
    )
    {
        _logger = logger;
        _gameRepository = gameRepository;
    }
    
    public async Task<Game> Execute(StartGameInput input)
    {
        IList<MazeArtifact> artifacts = new List<MazeArtifact>();
        Game game = new MazeGame(input.GameId, input.UserId, artifacts);
        ActiveGames.AddGame(game);
        return game;
    }
}