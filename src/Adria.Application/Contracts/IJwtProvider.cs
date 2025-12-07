using Adria.Domain.Users;

namespace Adria.Application.Contracts;

public interface IJwtProvider
{
    string GenerateToken(User user);
}