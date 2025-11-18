using Adria.Domain.Users;

namespace Adria.Application.Contracts.Data;

public sealed record UserData(User User, string JwtToken);
