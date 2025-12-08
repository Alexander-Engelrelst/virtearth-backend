using System.Security.Claims;
using Adria.Application.Contracts;
using Adria.Domain.games;
using Adria.Domain.Shared.Exceptions;
using Adria.Domain.Users;
using Adria.Infrastructure.Persistence.Shared;
using Adria.Infrastructure.WebApi.Controllers.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Adria.Infrastructure.WebApi.Controllers;

public static class GetGamesController
{
    public static async Task<Results<Ok<GameLocationDto[]>, ProblemHttpResult, BadRequest<string>, UnauthorizedHttpResult>> Invoke(
        [FromServices] IUseCase<Guid, Task<IReadOnlyCollection<GameLocation>>> getGames,
        [FromServices] IUseCase<Guid, Task<User>> getUser,
        [FromServices] IHttpContextAccessor httpContextAccessor
    )
    {
        Claim? userClaim = httpContextAccessor.HttpContext?.User.FindFirst("guid");

        if (userClaim is null || !Guid.TryParse(userClaim.Value, out Guid id))
        {
            return TypedResults.BadRequest("Please provide a valid user id");
        }

        try
        {
            await getUser.Execute(id);
        }
        catch (UserNotFoundException)
        {
            return TypedResults.Unauthorized();
        }
        
        
        try
        {
            IReadOnlyCollection<GameLocation> gameLocations = await getGames.Execute(id);
            return TypedResults.Ok(
                gameLocations.Select(location => new GameLocationDto(location)).ToArray()
                );
        }
        catch (VirtEarthDatabaseException)
        {
            return TypedResults.Problem(
                title: "Database Error",
                detail: "an unexpected database error has occured",
                statusCode: StatusCodes.Status500InternalServerError
            );
        }
    }
}