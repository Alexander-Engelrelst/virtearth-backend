namespace Adria.Application.Contracts;
public interface IUseCase<in Input, out Output>
{
    Output Execute(Input input);
}

public interface IUseCase<in Input>
{
    Task Execute(Input input);
}