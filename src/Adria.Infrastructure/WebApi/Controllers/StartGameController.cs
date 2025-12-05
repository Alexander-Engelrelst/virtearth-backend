using System.Security.Claims;
using Adria.Application.Contracts;
using Adria.Application.games;
using Adria.Domain.games;
using Adria.Domain.Shared.Exceptions;
using Adria.Domain.Users;
using Adria.Infrastructure.WebApi.Controllers.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Adria.Infrastructure.WebApi.Controllers;

public static class StartGameController
{
    public static async Task<Results<Ok<MazeGameDto>, UnauthorizedHttpResult, ProblemHttpResult, BadRequest<string>>> Invoke(
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
            return TypedResults.Unauthorized();
        }

        try
        {
            Game game = await startGame.Execute(new StartGameInput(gameId, user.Id));
            MazeGameDto response = new((MazeGame) game);
            return TypedResults.Ok(response);
        }
        catch (InvalidOperationException)
        {
            return TypedResults.Problem("An unexpected problem occured while trying to start a game");
        }
        
    }
}