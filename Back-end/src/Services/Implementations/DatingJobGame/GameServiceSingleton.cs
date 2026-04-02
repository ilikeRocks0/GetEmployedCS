using Back_end.Endpoints.Models;
using Back_end.Persistence.Interfaces;
using Back_end.Objects;
using Back_end.Services.Interfaces;

namespace Back_end.Services.Implementations;

public class GameServiceSingleton : IJobGameService
{
    private IJobGameService? jobGameService;

    public GameServiceSingleton(IServiceScopeFactory scopeFactory)
    {
        using var scope = scopeFactory.CreateScope();
        var userPersistence = scope.ServiceProvider.GetRequiredService<IUserPersistence>();
        var jobPersistence = scope.ServiceProvider.GetRequiredService<IJobPersistence>();
        var jobService = scope.ServiceProvider.GetRequiredService<IJobService>();
        
        jobGameService = new JobGameService(userPersistence, jobPersistence, jobService);
    }

    public Job? InitializeJobGame(CurrentUser currentUser)
    {
        if (jobGameService == null) throw new InvalidOperationException("Service not initialized.");
        return jobGameService.InitializeJobGame(currentUser);
    }

    public Job? AcceptJob(GameJob gameJob)
    {
        if (jobGameService == null) throw new InvalidOperationException("Game not initialized.");
        return jobGameService.AcceptJob(gameJob);
    }

    public Job? RejectJob(GameJob gameJob)
    {
        if (jobGameService == null) throw new InvalidOperationException("Game not initialized.");
        return jobGameService.RejectJob(gameJob);
    }

    public (int accepted, int rejected) GetGameStats(CurrentUser currentUser)
    {
        if (jobGameService == null) throw new InvalidOperationException("Game not initialized.");
        return jobGameService.GetGameStats(currentUser);
    } 
}