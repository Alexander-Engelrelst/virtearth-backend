using System.IdentityModel.Tokens.Jwt;

namespace Adria.Application.Contracts;
public interface IUseCase<in Input, out Output>
{
    Output Execute(Input input);
}

public interface IUseCase<out Output>
{
    Output Execute();
}