using Back_end.Endpoints.Models;
using Back_end.Persistence.Interfaces;
using Back_end.Objects;
using Back_end.Services.Interfaces;

namespace Back_end.Services.Implementations;

public class GameServiceSingleton : IJobGameService
{
    private IJobGameService? jobGameService;

    /// Initialize a job list for the game based on the provided filters. 
    /// <param name="currentUser">The user accessing the game.
    /// Returns a random job to start the game.
    public GameServiceSingleton(IServiceScopeFactory scopeFactory)
    {
        using var scope = scopeFactory.CreateScope();
        var userPersistence = scope.ServiceProvider.GetRequiredService<IUserPersistence>();
        var jobPersistence = scope.ServiceProvider.GetRequiredService<IJobPersistence>();
        var jobService = scope.ServiceProvider.GetRequiredService<IJobService>();
        
        jobGameService = new JobGameService(userPersistence, jobPersistence, jobService);
    }

    /// Initialize a job list for the game based on the provided filters. 
    /// <param name="currentUser">The user accessing the game.
    /// Returns a random job to start the game.
    public Job? InitializeJobGame(CurrentUser currentUser)
    {
        if (jobGameService == null) throw new InvalidOperationException("Service not initialized.");
        return jobGameService.InitializeJobGame(currentUser);
    }
    
    /// Accept the current job. The game statistics are updated to reflect the acceptance.    /// <param name="user">The user accessing the game.
    /// <param name="gameJob">The job being accepted.
    /// Returns the next job in the game.
    public Job? AcceptJob(GameJob gameJob)
    {
        if (jobGameService == null) throw new InvalidOperationException("Game not initialized.");
        return jobGameService.AcceptJob(gameJob);
    }

    /// Reject the current job. The game statistics are updated to reflect the rejection.
    /// <param name="gameJob">The job being rejected.
    /// Returns the next job in the game.
    public Job? RejectJob(GameJob gameJob)
    {
        if (jobGameService == null) throw new InvalidOperationException("Game not initialized.");
        return jobGameService.RejectJob(gameJob);
    }

    /// Get the current game statistics, including the number of accepted and rejected jobs.
    /// <param name="currentUser">The user accessing the game.
    /// Returns a tuple containing the number of accepted and rejected jobs.
    public (int accepted, int rejected) GetGameStats(CurrentUser currentUser)
    {
        if (jobGameService == null) throw new InvalidOperationException("Game not initialized.");
        return jobGameService.GetGameStats(currentUser);
    } 
}