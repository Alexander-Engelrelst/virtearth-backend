using System.Data.Common;
using Adria.Domain.games;
using Adria.Infrastructure.Persistence.Shared;
using Microsoft.Extensions.Logging;

namespace Adria.Infrastructure.Persistence.Repositories;

public sealed class AdoGameRepository : AbstractAdoRepository, IGameRepository
{
    private readonly ILogger<AdoGameRepository> _logger;
    
    public AdoGameRepository(
        DbProviderFactory factory,
        string connectionString,
        ILogger<AdoGameRepository> logger
    ) : base(factory, connectionString)
    {
        _logger = logger;
    }

    public MazeGameData GetMazeGameData(Guid id)
    {
        throw new NotImplementedException();
    }
}