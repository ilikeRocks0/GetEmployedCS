using Back_end.Endpoints.Models;
using Back_end.Persistence.Objects;

namespace Back_end.Services.Interfaces;

//Testable Logic component
public interface IJobGameConnector
{
    /// <summary>Initializes the job list for the game based on the provided filters and returns a random job to start the game. The filters are the same as those used in GetJobs.</summary>
    Job? InitializeJobGame(User user, IReadOnlyDictionary<string, string>? filters = null);
    
    /// <summary>Rejects the current job and returns the next job in the game. The game statistics are updated to reflect the rejection.</summary>
    public Job? RejectJob(User user, Job job);

    /// <summary>Accepts the current job and returns the next job in the game. The game statistics are updated to reflect the acceptance.</summary>
    public Job? AcceptJob(User user, Job job);

    /// <summary>Returns the current game statistics, including the number of accepted and rejected jobs.</summary>
    /// <returns>A tuple containing the number of accepted and rejected jobs.</returns>
    (int accepted, int rejected) GetGameStats(User user);
}
