using Adria.Application.Authentication;
using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Domain.Shared.Exceptions;
using Adria.Domain.Users;
using Microsoft.Extensions.Logging;

namespace Adria.Application.Users;

public class ChangeUserName : IUseCase<Guid, UserData>
{
    private readonly IUserRepository _repository;
    private readonly ILogger<CreateUser> _logger;
    private readonly IJwtProvider _jwtProvider;
    
    public ChangeUserName(
        IUserRepository repository,
        ILogger<CreateUser> logger,
        IJwtProvider jwtProvider
    )
    {
        _repository = repository;
        _logger = logger;
        _jwtProvider = jwtProvider;
    }
    public async Task<UserData> Execute(Guid id)
    {
        User? user = await _repository.ById(id);

        if (user is null) throw ElementNotFoundException.ForId<User>(id);
        
        
    }
}