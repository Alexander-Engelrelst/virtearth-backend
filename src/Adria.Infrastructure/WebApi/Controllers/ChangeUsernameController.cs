using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Application.Users;
using Adria.Domain.Shared.Exceptions;
using Adria.Infrastructure.Persistence.Shared;
using Adria.Infrastructure.WebApi.Controllers.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Adria.Infrastructure.WebApi.Controllers;

public static class ChangeUsernameController
{
    public static async Task<Results<Ok<UserDto>, NotFound<string>, ProblemHttpResult, Conflict<string>, BadRequest<string>>> Invoke(
        [FromRoute] Guid id,
        [FromQuery] string newUsername,
        [FromServices] IUseCase<ChangeUserNameInput, Task<UserData>> changeUsername
    )
    {
        try
        {
            UserData data = await changeUsername.Execute(new ChangeUserNameInput(id, newUsername));
            UserDto user = new(data.User.Id, data.User.Username, data.JwtToken);
            return TypedResults.Ok(user);
        }
        catch (ElementNotFoundException)
        {
            return TypedResults.NotFound($"User with id {id} not found");
        }
        catch (UsernameAlreadyExistsException)
        {
            return TypedResults.Conflict("Username already exists");
        }
        catch (InvalidUsernameException)
        {
            return TypedResults.BadRequest("Invalid username");
        }
        catch (ArgumentException)
        {
            return TypedResults.BadRequest("The username wasn't changed");
        }
        catch (VirtEarthDatabaseException)
        {
            return TypedResults.Problem("an unexpected error occured");
        }
    }
}