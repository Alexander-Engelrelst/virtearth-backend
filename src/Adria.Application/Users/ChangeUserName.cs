using Adria.Application.Authentication;
using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Domain.Shared.Exceptions;
using Adria.Domain.Users;
using Microsoft.Extensions.Logging;

namespace Adria.Application.Users;

public sealed record ChangeUserNameInput(User User, string NewName);
public sealed class ChangeUserName : IUseCase<ChangeUserNameInput, Task<UserData>>
{
    private readonly IUserRepository _repository;
    private readonly ILogger<ChangeUserName> _logger;
    private readonly IJwtProvider _jwtProvider;
    private readonly IUserExistsQuery _userExistsQuery;
    public ChangeUserName(
        IUserRepository repository,
        ILogger<ChangeUserName> logger,
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
        _logger.LogInformation("Executing user change request for id {Id}", input.User.Id);

        if (input.User.Username == input.NewName)
        {
            throw new ArgumentException("You cannot change your name to your current name", nameof(input.NewName));
        }
        
        
        bool usernameAlreadyExists = await _userExistsQuery.Fetch(input.NewName);
        if (usernameAlreadyExists)
        {
            _logger.LogError("User with name {Name} already exists", input.NewName);
            throw new UsernameAlreadyExistsException(input.NewName);
        }

        try
        {
            input.User.UpdateUserName(input.NewName);
        }
        catch (InvalidUsernameException ex)
        {
            _logger.LogError(ex, "Invalid username: {Username}", input.NewName);
            throw;
        }

        await _repository.Save(input.User);
        return new UserData(input.User, _jwtProvider.GenerateToken(input.User));
    }
}