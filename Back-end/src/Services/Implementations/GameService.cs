using Back_end.Persistence.Objects;
using Back_end.Services.Interfaces;

class GameService (IJobIndexManager jobIndexManager) : IGameService
{
    private int jobAccepted = 0;
    private int jobRejected = 0;

    List<Job> allJobs = [];
    private Dictionary<string, string>? currentFilters;

    // It retrieves the job at the current index from the list of all jobs, then increments the index and wraps it around to the start of the list when it reaches the end.    
    private Job GetGameJob()
    {
        // fetches the next page of jobs and shuffle them for the next round of the game.
        if (allJobs.Count == 0)
        {
            allJobs = jobIndexManager.GetJobs();
        }

        if (allJobs.Count == 0)
        {
            throw new InvalidOperationException("No jobs available for the game. Please initialize the job list first.");
        }

        Job job = allJobs[0];
        allJobs.RemoveAt(0);
        
        return job;
    }

    
    public Job InitializeJobGame(IReadOnlyDictionary<string, string>? filters = null)
    {
        currentFilters = filters?.ToDictionary(k => k.Key, v => v.Value) ?? [];
        jobIndexManager.UpdateJobsList(currentFilters);
        jobAccepted = 0;
        jobRejected = 0;

        return GetGameJob();
    }

    public Job RejectJob()
    {
        jobRejected++;
        return GetGameJob();
    }

    public Job AcceptJob()
    {
        jobAccepted++;
        return GetGameJob();
    }

    public (int accepted, int rejected) GetGameStats()
    {
        return (jobAccepted, jobRejected);
    }

}