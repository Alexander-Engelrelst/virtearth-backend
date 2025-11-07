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

public sealed class GetGamesController
{
    public static async Task<Results<Ok<UserDto>, BadRequest, Conflict, ProblemHttpResult>> Invoke(
        [FromServices] IUseCase<CreateUserInput, Task<UserData>> createUser
    )
    {

        throw new NotImplementedException();
    }
}