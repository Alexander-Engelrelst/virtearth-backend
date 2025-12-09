using System.Security.Claims;
using Adria.Application.Contracts;
using Adria.Application.games;
using Adria.Domain.games;
using Adria.Domain.Shared.Exceptions;
using Adria.Domain.Users;
using Adria.Infrastructure.Persistence.Shared;
using Adria.Infrastructure.WebApi.Controllers.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Adria.Infrastructure.WebApi.Controllers;

public static class StartGameController
{
    public static async Task<Results<Ok<MazeGameDto>, UnauthorizedHttpResult, ProblemHttpResult, Conflict<string>, BadRequest<string>>> Invoke(
        [FromServices] IUseCase<StartGameInput, Task<Game>> startGame,
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
            Game game = await startGame.Execute(new StartGameInput(gameId, user));
            MazeGameDto response = new((MazeGame)game);
            return TypedResults.Ok(response);
        }
        catch (PlayerAlreadyPlayingException)
        {
            return TypedResults.Conflict("Player already playing a game");
        }
        catch (VirtEarthDatabaseException)
        {
            return TypedResults.Problem("An unexpected problem occured while trying to start a game");
        }
        
    }
}