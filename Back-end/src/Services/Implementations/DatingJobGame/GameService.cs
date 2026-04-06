using Back_end.Objects;
using Back_end.Services.Interfaces;

namespace Back_end.Services.Implementations;

public class GameService (IJobIndexManager jobIndexManager) : IGameService
{
    private int jobAccepted = 0;
    private int jobRejected = 0;
    private bool isGameInitialized = false;
    private bool hasActiveJob = false;
    List<Job> allJobs = [];
    private Dictionary<string, string>? currentFilters;

    /// Retrieves the job at the current index from the list of all jobs, then increments the index and wraps it around to the start of the list when it reaches the end.
    /// <param name="initialize">A flag for whether the game has been initialized or not. If not provided, defaults to false.
    /// <param name="isAccepted">A flag for whether the last job has been accepted or not. If not provided, defaults to null.
    /// Returns the next job to display. 
    private Job? GetGameJob(bool initialize = false, bool? isAccepted = null)
    {
        if (!initialize && isAccepted.HasValue && hasActiveJob)
        {
            if (isAccepted.Value) 
            {
                jobAccepted++;
            }
            else
            {
                jobRejected++;
            }
            hasActiveJob = false;
        }
        // fetches the next page of jobs and shuffle them for the next round of the game.
        if (allJobs.Count == 0)
        {
            allJobs = jobIndexManager.GetJobs();
        }

        if (allJobs.Count == 0)
        {
            return null;
        } 

        Job job = allJobs[0];
        allJobs.RemoveAt(0);
        hasActiveJob = true;
        
        return job;
    }

    /// Initialize a job list for the game based on the provided filters. 
    /// <param name="filters">A dictionary of filter keys and values to apply when setting up the game.
    /// - The filters are the same as those used in GetJobs under JobService.
    /// Returns a random job to start the game.
    public Job? InitializeJobGame(IReadOnlyDictionary<string, string>? filters = null)
    {
        currentFilters = filters?.ToDictionary(k => k.Key, v => v.Value) ?? [];
        jobIndexManager.UpdateFilters(currentFilters);
        jobAccepted = 0;
        jobRejected = 0;
        isGameInitialized = true;
        hasActiveJob = false;
        allJobs.Clear();

        return GetGameJob(initialize: true);
    }

    /// Reject the current job.
    /// The game statistics are updated to reflect the rejection.
    /// Returns the next job in the game.
    public Job? RejectJob()
    {
        if (!isGameInitialized)
        {
            throw new InvalidOperationException("Game not initialized. Please call InitializeJobGame() before rejecting jobs.");
        }
        return GetGameJob(isAccepted: false);
    }

    /// Accept the current job.
    /// The game statistics are updated to reflect the acceptance.
    /// Returns the next job in the game.
    public Job? AcceptJob()
    {
        if (!isGameInitialized)
        {
            throw new InvalidOperationException("Game not initialized. Please call InitializeJobGame() before accepting jobs.");
        }
        return GetGameJob(isAccepted: true);
    }

    /// Get the current game statistics, including the number of accepted and rejected jobs.
    /// Returns a tuple containing the number of accepted and rejected jobs.
    public (int accepted, int rejected) GetGameStats()
    {
        if (!isGameInitialized)
        {
            throw new InvalidOperationException("Game not initialized. Please call InitializeJobGame() before getting game stats.");
        }
        return (jobAccepted, jobRejected);
    }

}