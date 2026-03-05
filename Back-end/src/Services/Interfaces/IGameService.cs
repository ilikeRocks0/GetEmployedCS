using Back_end.Persistence.Objects;

namespace Back_end.Services.Interfaces;

public interface IGameService
{
    /// <summary>Initializes the job list for the game based on the provided filters and returns a random job to start the game. The filters are the same as those used in GetJobs.</summary>
    Job? InitializeJobGame(IReadOnlyDictionary<string, string>? filters = null);
    
    /// <summary>Rejects the current job and returns the next job in the game. The game statistics are updated to reflect the rejection.</summary>
    public Job? RejectJob();

    /// <summary>Accepts the current job and returns the next job in the game. The game statistics are updated to reflect the acceptance.</summary>
    public Job? AcceptJob();

    /// <summary>Returns the current game statistics, including the number of accepted and rejected jobs.</summary>
    /// <returns>A tuple containing the number of accepted and rejected jobs.</returns>
    (int accepted, int rejected) GetGameStats();
}
