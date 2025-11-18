using System.IdentityModel.Tokens.Jwt;
using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Application.Users;
using Adria.Domain.Shared.Exceptions;
using Adria.Domain.Users;
using Adria.Infrastructure.Persistence.Shared;
using Adria.Infrastructure.WebApi.Controllers.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Adria.Infrastructure.WebApi.Controllers;

public static class LoginController
{
    public static async Task<Results<Ok<UserDto>, NotFound<string>, ProblemHttpResult>> Invoke(
        [FromRoute] Guid id,
        [FromServices] IUseCase<Guid, Task<UserData>> login
    )
    {
        try
        {
            UserData data = await login.Execute(id);
            UserDto user = new(data.User.Id, data.User.Username, data.JwtToken);
            return TypedResults.Ok(user);
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