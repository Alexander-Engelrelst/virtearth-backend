using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Application.Users;
using Adria.Domain.games;
using Adria.Domain.Shared.Exceptions;
using Adria.Infrastructure.Persistence.Shared;
using Adria.Infrastructure.WebApi.Controllers.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using GameLocation = Adria.Infrastructure.WebApi.Controllers.Responses.GameLocation;

namespace Adria.Infrastructure.WebApi.Controllers;

public static class GetGamesController
{
    public static async Task<Results<Ok<GameLocation[]>, ProblemHttpResult>> Invoke(
        [FromServices] IUseCase<Task<IReadOnlyCollection<Domain.games.GameLocation>>> getGames
    )
    {
        try
        {
            IReadOnlyCollection<Domain.games.GameLocation> gameLocations = await getGames.Execute();
            return TypedResults.Ok(
                gameLocations.Select(location => new GameLocation(location)).ToArray()
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