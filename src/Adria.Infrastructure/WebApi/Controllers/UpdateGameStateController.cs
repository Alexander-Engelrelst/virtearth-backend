using Adria.Application.Contracts;
using Adria.Application.games;
using Adria.Domain.games;
using Adria.Domain.Shared;
using Adria.Domain.Users;
using Adria.Infrastructure.Persistence.Shared;
using Adria.Infrastructure.WebApi.Controllers.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Adria.Infrastructure.WebApi.Controllers;

public static class UpdateGameStateController
{
    public static async Task<Results<Ok<MazeGameDto>, NoContent, UnauthorizedHttpResult, NotFound<string>, Conflict<string>>> Invoke(
        [FromServices] IUseCase<UpdateFoundMazeArtifactsInput, MazeGame?> updateFoundMazeArtifacts,
        [FromServices] IUseCase<Guid, Task<User>> getUser,
        [FromServices] IHttpContextAccessor httpContextAccessor,
        [FromRoute] Guid gameId,
        [FromRoute] Guid artifactId,
        [FromBody] UpdateGameStateBody body
    )
    {
        User user;

        try
        {
            user = await httpContextAccessor.GetUser(getUser);
        }
        catch (NoUserIdInTokenException)
        {
            return TypedResults.Unauthorized();
        }
        catch (UserNotFoundException)
        {
            return TypedResults.Unauthorized();
        }

        try
        {
            // TODO ask if this should be made async
            MazeGame? game = updateFoundMazeArtifacts.Execute(new UpdateFoundMazeArtifactsInput(
                user, artifactId, gameId, body.XCord, body.YCord, body.Angle
            ));

            if (game is null)
            {
                return TypedResults.NoContent();
            }
            
            return TypedResults.Ok(new MazeGameDto(game));
        }
        catch (ActiveGameNotFoundException)
        {
            return TypedResults.NotFound("The user has no active game");
        }
        catch (GameIdMismatchException)
        {
            return TypedResults.Conflict(
                "The id of the game the user is trying to update isn't the same as the id of the game being played");
        }
        catch (ArtifactAlreadyFoundException)
        {
            return TypedResults.Conflict("The artifact said to just be found was already found");
        }
        catch (ArtifactNotFoundException)
        {
            return TypedResults.NotFound("The artifact doesn't exist in your game");
        }
        
    }
}

public sealed record UpdateGameStateBody(
    float XCord,
    float YCord,
    float Angle
);