using Back_end.Endpoints.Models;
using Back_end.Objects;

namespace Back_end.Services.Interfaces;

//Testable Logic component
public interface IJobGameConnector
{
    /// Initialize a job list for the game based on the provided filters. 
    /// <param name="user">The user accessing the game.
    /// <param name="filters">A dictionary of filter keys and values to apply when setting up the game.
    /// - The filters are the same as those used in GetJobs under JobService.
    /// Returns a random job to start the game.
    Job? InitializeJobGame(User user, IReadOnlyDictionary<string, string>? filters = null);
    
    /// Reject the current job. The game statistics are updated to reflect the rejection.
    /// <param name="user">The user accessing the game.
    /// <param name="job">The job being rejected. 
    /// Returns the next job in the game.
    public Job? RejectJob(User user, Job job);

    /// Accept the current job. The game statistics are updated to reflect the acceptance.    /// <param name="user">The user accessing the game.
    /// <param name="user">The user accessing the game.
    /// <param name="job">The job being accepted. 
    /// Returns the next job in the game.
    public Job? AcceptJob(User user, Job job);

    /// Get the current game statistics, including the number of accepted and rejected jobs.
    /// Returns a tuple containing the number of accepted and rejected jobs.
    (int accepted, int rejected) GetGameStats(User user);
}
