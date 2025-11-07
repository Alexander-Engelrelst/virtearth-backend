using Adria.Application.Authentication;
using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Domain.Shared.Exceptions;
using Adria.Domain.Users;
using Microsoft.Extensions.Logging;

namespace Adria.Application.Users;

public sealed record ChangeUserNameInput(Guid Id, string NewName);
public sealed class ChangeUserName : IUseCase<ChangeUserNameInput, Task<UserData>>
{
    private readonly IUserRepository _repository;
    private readonly ILogger<CreateUser> _logger;
    private readonly IJwtProvider _jwtProvider;
    private readonly IUserExistsQuery _userExistsQuery;
    public ChangeUserName(
        IUserRepository repository,
        ILogger<CreateUser> logger,
        IJwtProvider jwtProvider,
        IUserExistsQuery userExistsQuery
    )
    {
        _repository = repository;
        _logger = logger;
        _jwtProvider = jwtProvider;
        _userExistsQuery = userExistsQuery;
    }
    public async Task<UserData> Execute(ChangeUserNameInput input)
    {
        User? user = await _repository.ById(input.Id);

        if (user is null) throw ElementNotFoundException.ForId<User>(input.Id);
        if (await _userExistsQuery.Fetch(input.NewName))
        {
            throw new UsernameAlreadyExistsException(input.NewName);
        }

        user.UpdateUserName(input.NewName);

        await _repository.Save(user);
        return new UserData(user, _jwtProvider.GenerateToken(user));
    }
}