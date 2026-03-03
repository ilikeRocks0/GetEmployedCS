using Back_end.Persistence.Objects;
using Back_end.Services.Interfaces;
using NUnit.Framework;

class GameService (IJobIndexManager jobIndexManager) : IGameService
{
    private int jobAccepted = 0;
    private int jobRejected = 0;
    private bool isGameInitialized = false;
    private bool hasActiveJob = false;
    List<Job> allJobs = [];
    private Dictionary<string, string>? currentFilters;

    // It retrieves the job at the current index from the list of all jobs, then increments the index and wraps it around to the start of the list when it reaches the end.    
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

    public Job? RejectJob()
    {
        if (!isGameInitialized)
        {
            throw new InvalidOperationException("Game not initialized. Please call InitializeJobGame() before rejecting jobs.");
        }
        return GetGameJob(isAccepted: false);
    }

    public Job? AcceptJob()
    {
        if (!isGameInitialized)
        {
            throw new InvalidOperationException("Game not initialized. Please call InitializeJobGame() before accepting jobs.");
        }
        return GetGameJob(isAccepted: true);
    }

    public (int accepted, int rejected) GetGameStats()
    {
        return (jobAccepted, jobRejected);
    }

}