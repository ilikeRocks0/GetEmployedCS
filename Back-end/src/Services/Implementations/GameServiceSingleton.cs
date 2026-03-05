using Back_end.Persistence.Objects;
using Back_end.Services.Interfaces;

namespace Back_end.Services.Implementations;

public class GameServiceSingleton(IServiceScopeFactory scopeFactory) : IGameService
{
    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;
    private GameService? _activeGame;

    public Job? InitializeJobGame(IReadOnlyDictionary<string, string>? filters = null)
    {
        using var scope = _scopeFactory.CreateScope();
        var manager = scope.ServiceProvider.GetRequiredService<IJobIndexManager>();
        
        _activeGame = new GameService(manager);
        return _activeGame.InitializeJobGame(filters);
    }

    public Job? AcceptJob()
    {
        if (_activeGame == null) throw new InvalidOperationException("Game not initialized.");
        return _activeGame.AcceptJob();
    }

    public Job? RejectJob()
    {
        if (_activeGame == null) throw new InvalidOperationException("Game not initialized.");
        return _activeGame.RejectJob();
    }

    public (int accepted, int rejected) GetGameStats()
    {
        if (_activeGame == null) throw new InvalidOperationException("Game not initialized.");
        return _activeGame.GetGameStats();
    } 
}