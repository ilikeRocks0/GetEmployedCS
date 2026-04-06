using Back_end.Persistence.Interfaces;
using Back_end.Objects;
using Back_end.Services.Interfaces;

namespace Back_end.Services.Implementations;

public class ShuffleJobsService: IJobIndexManager
{
    private readonly IJobIndexManager jobIndexManager;

    public ShuffleJobsService(IJobService jobService)
    {
        jobIndexManager = new JobIndexManager(jobService);
    }

    /// Get a list of jobs. 
    /// Returns a list of all jobs using existing filtration.
    public List<Job> GetJobs()
    {
        return ShuffleJobs(jobIndexManager.GetJobs());
    }

    /// Update a list of filters used to retrieve jobs. 
    /// <param name="filters">A dictionary of filter keys and values to apply when retrieving jobs.
    public void UpdateFilters(IReadOnlyDictionary<string, string>? filters)
    {
        jobIndexManager.UpdateFilters(filters);
    }
    
    /// Get a shuffled version of a list of jobs. 
    /// <param name="jobs">The list of jobs to shuffle.
    /// Returns a shuffled version of the jobs parameter.
    private List<Job> ShuffleJobs(List<Job> jobs)
    {
        Random rand = new();
        int n = jobs.Count;
        for (int i = 0; i < n; i++)
        {
            int j = rand.Next(i, n);
            (jobs[j], jobs[i]) = (jobs[i], jobs[j]);
        }
        return jobs;
    }

}