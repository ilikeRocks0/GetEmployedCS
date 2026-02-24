using Back_end.Persistence.Objects;
using Back_end.Services.Interfaces;

public class JobIndexManager (IJobService jobService): IJobIndexManager
{

    private int currentPage = 1;
    
    private readonly List<Job> allJobs = [];

    private Dictionary<string, string> filtersDictionary = [];

    public List<Job> GetJobs()
    {
        currentPage += 1;
        allJobs.Clear();
        allJobs.AddRange(jobService.GetJobs(filtersDictionary).ToList());
        return allJobs;
    }

    public void UpdateJobsList(IReadOnlyDictionary<string, string>? filters)
    {
        allJobs.Clear();
        Dictionary<string, string> filtersDictionary = filters?.ToDictionary(k => k.Key, v => v.Value) ?? [];
        allJobs.AddRange(jobService.GetJobs(filtersDictionary).ToList());
    }
}