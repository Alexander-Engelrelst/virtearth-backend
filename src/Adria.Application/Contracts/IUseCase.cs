using System.IdentityModel.Tokens.Jwt;
using Adria.Application.Contracts.Data;
using Adria.Domain.games;

namespace Adria.Application.Contracts;
public interface IUseCase<in Input, out Output>
{
    Output Execute(Input input);
}

public interface IUseCase<out Output>
{
    Task<IReadOnlyCollection<GameLocation>> Execute();
}