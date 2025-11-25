using System.IdentityModel.Tokens.Jwt;
using Adria.Domain.Users;

namespace Adria.Application.Authentication;

public interface IJwtProvider
{
    string GenerateToken(User user);
}