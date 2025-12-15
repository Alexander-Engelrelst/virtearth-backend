using Adria.Application.Contracts;
using Adria.Domain.games;
using Adria.Domain.Shared;
using Adria.Domain.Users;
using Microsoft.Extensions.Logging;

namespace Adria.Application.games;

public sealed record UpdateFoundMazeArtifactsInput(User User, Guid ArtifactId, Guid GameId, float XCord, float YCord, float Angle);
public sealed class UpdateFoundMazeArtifacts(ILogger<UpdateFoundMazeArtifacts> logger) : IUseCase<UpdateFoundMazeArtifactsInput, Task<MazeGame?>>
{
    private readonly ILogger<UpdateFoundMazeArtifacts> _logger = logger;
    public Task<MazeGame?> Execute(UpdateFoundMazeArtifactsInput input)
    {
        /* here we will once again not make a distinction between types of games although this is in the database,
         * for an explanation as of why I refer to the StartGame Usecase where this is explained */
        _logger.LogInformation("trying to add artifact {ArtifactId} as found for {UserId}.", input.ArtifactId, input.User.Id);
        MazeGame game = (MazeGame) ActiveGames.Get(input.User.Id);
        
        if (game.GameId != input.GameId)
        {
            throw new GameIdMismatchException(input.User.Id);
        }
        
        return Task.FromResult(game.UpdateUserFoundArtifacts(input.ArtifactId, input.XCord,  input.YCord, input.Angle));
    }
}