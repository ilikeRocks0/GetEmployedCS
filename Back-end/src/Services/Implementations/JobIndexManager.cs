using Back_end.Persistence.Objects;
using Back_end.Services.Interfaces;
using Back_end.Util;

public class JobIndexManager(IJobService jobService) : IJobIndexManager
{

    private int currentPage = 1;
    
    private readonly List<Job> allJobs = [];

    private Dictionary<string, string> filtersDictionary = [];

    public List<Job> GetJobs()
    {
        allJobs.Clear();
        filtersDictionary[AppConfig.FilterKeys.PAGE_NUMBER] = currentPage.ToString();
        allJobs.AddRange(jobService.GetJobs(filtersDictionary).jobList.ToList());
        currentPage += 1;
        return allJobs;
    }

    public void UpdateFilters(IReadOnlyDictionary<string, string>? filters)
    {
        filtersDictionary = filters?.ToDictionary(k => k.Key, v => v.Value) ?? [];
    }
}