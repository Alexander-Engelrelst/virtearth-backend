using Adria.Application.Contracts;
using Adria.Application.games;
using Adria.Domain.Shared;
using Adria.Domain.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Adria.Infrastructure.WebApi.Controllers;

public class UpdateTtlController
{
    public static async Task<Results<UnauthorizedHttpResult, NoContent, NotFound<string>, Conflict<string>>> Invoke(
        [FromServices] IUseCase<UpdateTtlInput> updateTtl,
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
            await updateTtl.Execute(new UpdateTtlInput(user, gameId));
            return TypedResults.NoContent();
        }
        catch (ActiveGameNotFoundException)
        {
            return TypedResults.NotFound("The user is not playing a game");
        }
        catch (GameIdMismatchException)
        {
            return TypedResults.Conflict(
                "The id of the game the user is trying to update isn't the same as the id of the game being played");
        }
    }
}
