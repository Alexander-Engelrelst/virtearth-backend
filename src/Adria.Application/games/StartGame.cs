using System.Diagnostics;
using Adria.Application.Contracts;
using Adria.Domain.games;
using Microsoft.Extensions.Logging;

namespace Adria.Application.games;

public sealed record StartGameInput(Guid GameId, Guid UserId);

public sealed class StartGame : IUseCase<StartGameInput, Task<Game>>
{
    private readonly ILogger<StartGame> _logger;
    private readonly IGameRepository _gameRepository;
    private readonly IArtifactsQuery  _artifactsQuery;
    public StartGame(
        ILogger<StartGame> logger,
        IGameRepository gameRepository,
        IArtifactsQuery artifactsQuery
    )
    {
        _logger = logger;
        _gameRepository = gameRepository;
        _artifactsQuery = artifactsQuery;
    }
    
    public async Task<Game> Execute(StartGameInput input)
    {
        IList<MazeArtifact> artifacts = await _artifactsQuery.Fetch(input.GameId);
        Game game = new MazeGame(input.GameId, input.UserId, artifacts);
        ActiveGames.AddGame(game);
        return game;
    }
}