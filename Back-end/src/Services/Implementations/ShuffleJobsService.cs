using Back_end.Persistence.Interfaces;
using Back_end.Persistence.Objects;
using Back_end.Services.Interfaces;

public class ShuffleJobsService: IJobIndexManager
{
    private readonly IJobIndexManager jobIndexManager;

    public ShuffleJobsService(IJobService jobService)
    {
        jobIndexManager = new JobIndexManager(jobService);
    }

    public List<Job> GetJobs()
    {

        return ShuffleJobs(jobIndexManager.GetJobs());

    }

    public void UpdateFilters(IReadOnlyDictionary<string, string>? filters)
    {
        jobIndexManager.UpdateFilters(filters);
    }
    
    // Shuffles the list of all jobs for the game. 
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