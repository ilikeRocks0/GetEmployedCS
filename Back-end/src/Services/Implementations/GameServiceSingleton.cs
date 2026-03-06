using Back_end.Endpoints.Models;
using Back_end.Persistence.Interfaces;
using Back_end.Persistence.Objects;
using Back_end.Services.Interfaces;

namespace Back_end.Services.Implementations;

public class GameServiceSingleton(IServiceScopeFactory scopeFactory) : IJobGameConnector
{
    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;
    private JobGameConnector? activeGame;

    public Job? InitializeJobGame(CurrentUser currentUser, IReadOnlyDictionary<string, string>? filters = null)
    {
        using var scope = _scopeFactory.CreateScope();
        var userPersistence = scope.ServiceProvider.GetRequiredService<IUserPersistence>();
        var jobPersistence = scope.ServiceProvider.GetRequiredService<IJobPersistence>();
        var jobService = scope.ServiceProvider.GetRequiredService<IJobService>();
        
        activeGame = new JobGameConnector(userPersistence, jobPersistence, jobService);
        return activeGame.InitializeJobGame(currentUser, filters);
    }

    public Job? AcceptJob(GameJob gameJob)
    {
        if (activeGame == null) throw new InvalidOperationException("Game not initialized.");
        return activeGame.AcceptJob(gameJob);
    }

    public Job? RejectJob(GameJob gameJob)
    {
        if (activeGame == null) throw new InvalidOperationException("Game not initialized.");
        return activeGame.RejectJob(gameJob);
    }

    public (int accepted, int rejected) GetGameStats(CurrentUser currentUser)
    {
        if (activeGame == null) throw new InvalidOperationException("Game not initialized.");
        return activeGame.GetGameStats(currentUser);
    } 
}