using Adria.Application.Contracts;
using Adria.Application.Users;
using Adria.Domain.Shared.Exceptions;
using Adria.Domain.Users;
using Adria.Infrastructure.Persistence.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Adria.Infrastructure.WebApi.Controllers;

public class CreateUserController
{
    public static async Task<Results<Ok<User>, BadRequest, Conflict, ProblemHttpResult>> Invoke(
        [FromBody] CreateUserBody body,
        [FromServices] IUseCase<CreateUserInput, Task<User>> createUser
    )
    {
        Avatar? avatar = Enum.TryParse(body.Avatar, true, out Avatar result) ? result : null;

        try
        {
            return TypedResults.Ok(await createUser.Execute(new CreateUserInput(body.Username, avatar)));
        }
        catch (InvalidUsernameException ex)
        {
            return TypedResults.BadRequest();
        }
        catch (UsernameAlreadyExistsException ex)
        {
            return TypedResults.Conflict();
        }
        catch (VirtEarthDatabaseException ex)
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
    string Username,
    string? Avatar
);