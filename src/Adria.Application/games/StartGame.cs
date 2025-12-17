using System.Collections.Immutable;
using Adria.Application.Contracts;
using Adria.Domain.games;
using Adria.Domain.Shared;
using Adria.Domain.Users;
using Microsoft.Extensions.Logging;

namespace Adria.Application.games;

public sealed record StartGameInput(Guid GameId, User User);

public sealed class StartGame : IUseCase<StartGameInput, Task<Game>>
{
    private readonly ILogger<StartGame> _logger;
    private readonly IArtifactsQuery  _artifactsQuery;
    public StartGame(
        ILogger<StartGame> logger,
        IArtifactsQuery artifactsQuery
    )
    {
        _logger = logger;
        _artifactsQuery = artifactsQuery;
    }
    
    public async Task<Game> Execute(StartGameInput input)
    {
        _logger.LogInformation("Starting game {GameId} for user {UserId}", input.GameId, input.User.Id);
        /* normally this would be made more robust and safe for other types of games,
         * I was planning on doing this first but second guessed my decision until I was able to ask if this was expected
         * I was told during the code review that I did not have to do this, so I didn't, but I also didn't refactor everything to simplify*/
        IReadOnlySet<MazeArtifact> artifacts = await _artifactsQuery.Fetch(input.GameId);
        Game game = new MazeGame(input.GameId, input.User, artifacts);
        
        ActiveGames.AddGame(game);
        
        return game;
    }
}