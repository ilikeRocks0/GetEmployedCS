using Back_end.Endpoints.Models;
using Back_end.Persistence.Interfaces;
using Back_end.Persistence.Objects;
using Back_end.Services.Interfaces;

namespace Back_end.Services.Implementations;

public class GameServiceSingleton : IJobGameService
{
    private IJobGameService? server;

    public GameServiceSingleton(IServiceScopeFactory scopeFactory)
    {
        using var scope = scopeFactory.CreateScope();
        var userPersistence = scope.ServiceProvider.GetRequiredService<IUserPersistence>();
        var jobPersistence = scope.ServiceProvider.GetRequiredService<IJobPersistence>();
        var jobService = scope.ServiceProvider.GetRequiredService<IJobService>();
        
        server = new JobGameService(userPersistence, jobPersistence, jobService);
    }

    public Job? InitializeJobGame(CurrentUser currentUser, IReadOnlyDictionary<string, string>? filters = null)
    {
        if (server == null) throw new InvalidOperationException("Service not initialized.");
        return server.InitializeJobGame(currentUser, filters);
    }

    public Job? AcceptJob(GameJob gameJob)
    {
        if (server == null) throw new InvalidOperationException("Game not initialized.");
        return server.AcceptJob(gameJob);
    }

    public Job? RejectJob(GameJob gameJob)
    {
        if (server == null) throw new InvalidOperationException("Game not initialized.");
        return server.RejectJob(gameJob);
    }

    public (int accepted, int rejected) GetGameStats(CurrentUser currentUser)
    {
        if (server == null) throw new InvalidOperationException("Game not initialized.");
        return server.GetGameStats(currentUser);
    } 
}