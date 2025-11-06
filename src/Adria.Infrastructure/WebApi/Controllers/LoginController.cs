using System.IdentityModel.Tokens.Jwt;
using Adria.Application.Contracts;
using Adria.Application.Users;
using Adria.Domain.Shared.Exceptions;
using Adria.Infrastructure.Persistence.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Adria.Infrastructure.WebApi.Controllers;

public class LoginController
{
    public static async Task<Results<Ok<string>, NotFound<string>, ProblemHttpResult>> Invoke(
        [FromRoute] Guid id,
        [FromServices] IUseCase<Guid, Task<string>> login
    )
    {
        try
        {
            return TypedResults.Ok(await login.Execute(id));
        }
        catch (ElementNotFoundException)
        {
            return TypedResults.NotFound($"User with id {id} not found");
        }
        catch (VirtEarthDatabaseException)
        {
            return TypedResults.Problem("an unexpected error occured");
        }
    }
}