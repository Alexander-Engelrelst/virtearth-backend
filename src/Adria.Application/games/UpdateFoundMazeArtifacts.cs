using Adria.Application.Contracts;
using Adria.Domain.games;
using Adria.Domain.Shared.Exceptions;
using Microsoft.Extensions.Logging;

namespace Adria.Application.games;

public sealed record UpdateFoundMazeArtifactsInput(Guid UserId, Guid ArtifactId, Guid GameId);
public sealed class UpdateFoundMazeArtifacts(ILogger<UpdateFoundMazeArtifacts> logger) : IUseCase<UpdateFoundMazeArtifactsInput>
{
    private readonly ILogger<UpdateFoundMazeArtifacts> _logger = logger;
    public void Execute(UpdateFoundMazeArtifactsInput input)
    {
        /* here we will once again not make a distinction between types of games although this is in the database,
         * for an explanation as of why I refer to the StartGame Usecase where this is explained */
        _logger.LogInformation("trying to add artifact {ArtifactId} as found for {UserId}.", input.ArtifactId, input.UserId);
        MazeGame game = (MazeGame) ActiveGames.Get(input.UserId);
        
        if (game.GameId != input.GameId)
        {
            throw new GameIdMismatchException(input.UserId);
        }
        
        game.UpdateUserFoundArtifacts(input.ArtifactId);
    }
}