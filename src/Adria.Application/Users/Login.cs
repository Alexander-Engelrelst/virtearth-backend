using System.IdentityModel.Tokens.Jwt;
using Adria.Application.Contracts;

namespace Adria.Application.Users;

public sealed record LoginInput(Guid Id, string Username);

public sealed class Login : IUseCase<LoginInput, JwtSecurityToken>
{
    public JwtSecurityToken Execute(LoginInput input)
    {
        throw new NotImplementedException();
    }
}