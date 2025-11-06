using System.IdentityModel.Tokens.Jwt;
using Adria.Application.Contracts;
using Adria.Application.Users;
using Microsoft.AspNetCore.Mvc;

namespace Adria.Infrastructure.WebApi.Controllers.Responses;

public class LoginController
{
    public static async Task<JwtSecurityToken> Invoke(
        [FromBody] LoginBody body,
        [FromServices] IUseCase<LoginInput, JwtSecurityToken> login
    )
    {
        try
        {
            return login.Execute(new LoginInput(body.Id, body.Username));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}

public sealed record LoginBody(
    Guid Id,
    string Username
    );