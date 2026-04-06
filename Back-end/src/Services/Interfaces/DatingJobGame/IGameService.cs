using Back_end.Objects;

namespace Back_end.Services.Interfaces;

public interface IGameService
{
    /// Initialize a job list for the game based on the provided filters. 
    /// <param name="filters">A dictionary of filter keys and values to apply when setting up the game.
    /// - The filters are the same as those used in GetJobs under JobService.
    /// Returns a random job to start the game.
    Job? InitializeJobGame(IReadOnlyDictionary<string, string>? filters = null);
    
    /// Reject the current job.
    /// The game statistics are updated to reflect the rejection.
    /// Returns the next job in the game.
    public Job? RejectJob();

    /// Accept the current job.
    /// The game statistics are updated to reflect the acceptance.
    /// Returns the next job in the game.
    public Job? AcceptJob();

    /// Get the current game statistics, including the number of accepted and rejected jobs.
    /// Returns a tuple containing the number of accepted and rejected jobs.
    (int accepted, int rejected) GetGameStats();
}
