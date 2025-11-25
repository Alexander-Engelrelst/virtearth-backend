using System.Security.Claims;
using Adria.Application.Contracts;
using Adria.Application.games;
using Adria.Domain.games;
using Adria.Domain.Shared.Exceptions;
using Adria.Domain.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Adria.Infrastructure.WebApi.Controllers;

public static class StartGameController
{
    public static async Task<Results<Ok<Game>, NotFound<string>, ProblemHttpResult, BadRequest<string>>> Invoke(
        [FromServices] IUseCase<StartGameInput, Task<Game>> startGame,
        [FromServices] IUseCase<Guid, Task<User>> getUser,
        [FromServices] IHttpContextAccessor httpContextAccessor,
        [FromRoute] Guid gameId
        )
    {
        Claim? userClaim = httpContextAccessor.HttpContext?.User.FindFirst("guid");

        if (userClaim is null || !Guid.TryParse(userClaim.Value, out Guid id))
        {
            return TypedResults.BadRequest("Please provide a valid user id");
        }

        User user;
        try
        {
            user = await getUser.Execute(id);
        }
        catch (ElementNotFoundException)
        {
            return TypedResults.NotFound("There is no user with the given id");
        }

        try
        {
            Game game = await startGame.Execute(new StartGameInput(gameId, user.Id));
            // TODO use a gameDto because the maze can't be serialized
            return TypedResults.Ok(game);
        }
        catch (InvalidOperationException)
        {
            return TypedResults.Problem("An unexpected problem occured while trying to start a game");
        }
        
    }
}