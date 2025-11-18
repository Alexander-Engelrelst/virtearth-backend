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

public sealed class GetGamesController
{
    public static async Task<Results<Ok<IReadOnlyCollection<GameLocation>>, ProblemHttpResult>> Invoke(
        [FromServices] IUseCase<IReadOnlyCollection<GameLocation>> getGames,
        [FromQuery] Guid id
    )
    {
        try
        {
            return TypedResults.Ok(await getGames.Execute());
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