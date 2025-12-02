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

namespace Adria.Infrastructure.WebApi.Controllers;

public static class GetGamesController
{
    public static async Task<Results<Ok<GameLocationDto[]>, ProblemHttpResult>> Invoke(
        [FromServices] IUseCase<Task<IReadOnlyCollection<GameLocation>>> getGames
    )
    {
        try
        {
            IReadOnlyCollection<GameLocation> gameLocations = await getGames.Execute();
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