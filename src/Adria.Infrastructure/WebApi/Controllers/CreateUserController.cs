using System.Data.Common;
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
using MySql.Data.MySqlClient;

namespace Adria.Infrastructure.WebApi.Controllers;

public static class CreateUserController
{
    public static async Task<Results<Ok<UserDto>, BadRequest, Conflict<string>, ProblemHttpResult>> Invoke(
        [FromBody] CreateUserBody body,
        [FromServices] IUseCase<CreateUserInput, Task<UserData>> createUser
    )
    {
        try
        {
            UserData data = await createUser.Execute(new CreateUserInput(body.Username));
            return TypedResults.Ok(new UserDto(data.User.Id, data.User.Username, data.JwtToken));
        }
        catch (InvalidUsernameException)
        {
            return TypedResults.BadRequest();
        }
        catch (UsernameAlreadyExistsException)
        {
            return TypedResults.Conflict("username already exists");
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

public sealed record CreateUserBody(
    string Username
);