using Adria.Application.Contracts;
using Adria.Application.games;
using Adria.Domain.Shared.Exceptions;
using Adria.Domain.Users;
using Adria.Infrastructure.Persistence.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Adria.Infrastructure.WebApi.Controllers;

public class SaveGameController
{
    public static async Task<Results<UnauthorizedHttpResult, NotFound<string>, Conflict<string>, NoContent, ProblemHttpResult>> Invoke(
        [FromServices] IUseCase<SaveFinishedGameInput> saveGame,
        [FromServices] IUseCase<Guid, Task<User>> getUser,
        [FromServices] IHttpContextAccessor httpContextAccessor,
        [FromRoute] Guid gameId
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
            await saveGame.Execute(new SaveFinishedGameInput(user, gameId));
            return TypedResults.NoContent();
        }
        catch (GameIdMismatchException)
        {
            return TypedResults.Conflict(
                "The id of the game the user is trying to update isn't the same as the id of the game being played");
        }
        catch (ActiveGameNotFoundException)
        {
            return TypedResults.NotFound("The user has no active game");
        }
        catch (GameNotFinishedException)
        {
            return TypedResults.Conflict("The user has not finished yet");
        }
        catch (GameAlreadyCompletedByUserException)
        {
            /* this means the player already completed this game, this is expected behaviour
             * we still need the request to be able to update the cache */
            return TypedResults.NoContent();
        }
        catch (VirtEarthDatabaseException)
        {
            return TypedResults.Problem("an unexpected error occured");
        }
    }
}