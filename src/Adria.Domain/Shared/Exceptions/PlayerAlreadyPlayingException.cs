namespace Adria.Domain.Shared.Exceptions;

public sealed class PlayerAlreadyPlayingException(Guid userId, Exception? innerException = null) : Exception(
    $"username {userId} already playing a game."
    , innerException)
{
}