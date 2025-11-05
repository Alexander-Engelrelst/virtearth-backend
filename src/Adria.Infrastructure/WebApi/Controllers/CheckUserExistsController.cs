using Adria.Application.Contracts;
using Adria.Application.Users;
using Adria.Domain.Shared.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Adria.Infrastructure.WebApi.Controllers;

public class CheckUserExistsController
{
    public static async Task<Results<NoContent, Conflict, BadRequest>> Invoke(
        [FromQuery] string username,
        [FromServices]
        IUseCase<CheckUsernameInUseInput, Task<bool>> checkUserExists
    )
    {
        try
        {
            if (await checkUserExists.Execute(new CheckUsernameInUseInput(username)))
            {
                return TypedResults.Conflict();
            }
            else
            {
                return TypedResults.NoContent();

            }
        }
        catch (InvalidUsernameException ex)
        {
            return TypedResults.BadRequest();
        }
    }
}