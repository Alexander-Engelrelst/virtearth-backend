using System.IdentityModel.Tokens.Jwt;
using Adria.Application.Contracts;
using Adria.Application.Users;
using Microsoft.AspNetCore.Mvc;

namespace Adria.Infrastructure.WebApi.Controllers.Responses;

public class LoginController
{
    public static async Task<string> Invoke(
        [FromRoute] Guid id,
        [FromServices] IUseCase<Guid, Task<string>> login
    )
    {
        try
        {
            return await login.Execute(id);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}