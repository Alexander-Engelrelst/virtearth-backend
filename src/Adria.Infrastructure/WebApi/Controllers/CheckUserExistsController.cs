using Adria.Application.Contracts;
using Adria.Application.Users;
using Adria.Domain.Shared.Exceptions;
using Adria.Infrastructure.Persistence.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Adria.Infrastructure.WebApi.Controllers;

public static class CheckUserExistsController
{
    public static async Task<Results<NoContent, Conflict<string>, BadRequest<string>, ProblemHttpResult>> Invoke(
        [FromQuery] string username,
        [FromServices]
        IUseCase<CheckUsernameInUseInput, Task<bool>> checkUserExists
    )
    {
        try
        {
            if (await checkUserExists.Execute(new CheckUsernameInUseInput(username)))
            {
                return TypedResults.Conflict("Username already exists");
            }
            else
            {
                return TypedResults.NoContent();

            }
        }
        catch (InvalidUsernameException)
        {
            return TypedResults.BadRequest("Invalid username");
        }
        catch (VirtEarthDatabaseException)
        {
            return TypedResults.Problem("an unexpected error occured");
        }
    }
}