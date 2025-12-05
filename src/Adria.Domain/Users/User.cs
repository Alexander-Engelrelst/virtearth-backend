using System.Text.RegularExpressions;
using Adria.Domain.Shared.Exceptions;

namespace Adria.Domain.Users;

public sealed partial class User
{   
    private const int MAXIMUM_USERNAME_LENGTH = 40;
    private const int MINIMUM_USERNAME_LENGTH = 3;
    [GeneratedRegex(@"^[a-zA-Z0-9._-]{3,40}$")]
    private static partial Regex UsernameRegex(); 
    public Guid Id { get; private init; }
    public string Username { get; private set; }

    public User(
        string username,
        Guid userId = default
    )
    {
        EnsureValidUsername(username);
        
        Id = userId == Guid.Empty ?  Guid.NewGuid() : userId;
        Username = username;
    }

    public static void EnsureValidUsername(string username)
    {
        if (string.IsNullOrWhiteSpace(username) ||
            username.Length < MINIMUM_USERNAME_LENGTH ||
            username.Length > MAXIMUM_USERNAME_LENGTH ||
            !UsernameRegex().IsMatch(username))
        {
            throw new InvalidUsernameException(username);
        }
    }

    public void UpdateUserName(string inputNewName)
    {
        EnsureValidUsername(inputNewName);
        
        Username =  inputNewName;
    }

    private bool Equals(User other)
    {
        return Id.Equals(other.Id);
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is User other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}