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
        _logger.LogInformation("Executing user change request for id {id}", input.Id);
        User? user = await _repository.ById(input.Id);

        if (user is null)
        {
            _logger.LogError("User with id {id} was not found", input.Id);
            throw ElementNotFoundException.ForId<User>(input.Id);
        }
        
        bool usernameAlreadyExists = await _userExistsQuery.Fetch(input.NewName);
        if (usernameAlreadyExists)
        {
            _logger.LogError("User with name {name} already exists", input.NewName);
            throw new UsernameAlreadyExistsException(input.NewName);
        }

        try
        {
            user.UpdateUserName(input.NewName);
        }
        catch (InvalidUsernameException)
        {
            _logger.LogError("Invalid username: {username}", input.NewName);
        }

        await _repository.Save(user);
        return new UserData(user, _jwtProvider.GenerateToken(user));
    }
}