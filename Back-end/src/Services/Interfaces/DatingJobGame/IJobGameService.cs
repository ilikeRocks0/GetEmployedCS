using Back_end.Endpoints.Models;
using Back_end.Objects;

namespace Back_end.Services.Interfaces;

//Handles Front-end endpoints
public interface IJobGameService
{
    /// Initialize a job list for the game based on the provided filters. 
    /// <param name="currentUser">The user accessing the game.
    /// Returns a random job to start the game.
    Job? InitializeJobGame(CurrentUser currentUser);
    
    /// Reject the current job. The game statistics are updated to reflect the rejection.
    /// <param name="gameJob">The job being rejected.
    /// Returns the next job in the game.
    public Job? RejectJob(GameJob gameJob);

    /// Accept the current job. The game statistics are updated to reflect the acceptance.    /// <param name="user">The user accessing the game.
    /// <param name="gameJob">The job being accepted.
    /// Returns the next job in the game.
    public Job? AcceptJob(GameJob gameJob);

    /// Get the current game statistics, including the number of accepted and rejected jobs.
    /// <param name="currentUser">The user accessing the game.
    /// Returns a tuple containing the number of accepted and rejected jobs.
    (int accepted, int rejected) GetGameStats(CurrentUser currentUser);
}
