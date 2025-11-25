using System.Security.Claims;
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

namespace Adria.Infrastructure.WebApi.Controllers;

public static class ChangeUsernameController
{
    public static async Task<Results<Ok<UserDto>, NotFound<string>, ProblemHttpResult, Conflict<string>, BadRequest<string>>> Invoke(
        [FromQuery] string newUsername,
        [FromServices] IUseCase<Guid, Task<User>> getUser,
        [FromServices] IUseCase<ChangeUserNameInput, Task<UserData>> changeUsername,
        [FromServices] IHttpContextAccessor httpContextAccessor
    )
    {
        Claim? userClaim = httpContextAccessor.HttpContext?.User.FindFirst("guid");

        if (userClaim is null || !Guid.TryParse(userClaim.Value, out Guid id))
        {
            return TypedResults.BadRequest("Please provide a valid user id");
        }

        User user;
        try
        {
            user = await getUser.Execute(id);
        }
        catch (ElementNotFoundException)
        {
            return TypedResults.NotFound("There is no use with the given id");
        }

        try
        {
            UserData data = await changeUsername.Execute(new ChangeUserNameInput(user, newUsername));
            UserDto userDto = new(data.User.Id, data.User.Username, data.JwtToken);
            return TypedResults.Ok(userDto);
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