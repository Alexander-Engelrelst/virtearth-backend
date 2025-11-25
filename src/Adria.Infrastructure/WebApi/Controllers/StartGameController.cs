using Adria.Infrastructure.WebApi.Controllers.Responses;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adria.Infrastructure.WebApi.Controllers;

public static class StartGameController
{
    public static async Task<Results<Ok<UserDto>, NotFound<string>, ProblemHttpResult>> Invoke(

    )
    {
        // TODO yk what to do lol
        throw new NotImplementedException("secondj");
    }
}