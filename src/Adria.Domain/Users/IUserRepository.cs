namespace Adria.Domain.Users;

public interface IUserRepository
{
    Task Save(User user);
    
    Task<User?> ById(Guid id);
}