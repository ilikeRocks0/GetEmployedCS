using Back_end.Objects;
using Back_end.Services.Interfaces;
using Back_end.Util;

namespace Back_end.Services.Implementations;

public class JobIndexManager(IJobService jobService) : IJobIndexManager
{

    private int currentPage = 1;
    
    private readonly List<Job> allJobs = [];

    private Dictionary<string, string> filtersDictionary = [];

    /// Get a list of jobs. 
    /// Returns a list of all jobs using existing filtration.
    public List<Job> GetJobs()
    {
        allJobs.Clear();
        filtersDictionary[AppConfig.FilterKeys.PAGE_NUMBER] = currentPage.ToString();
        allJobs.AddRange(jobService.GetJobs(filtersDictionary).ToList());
        currentPage += 1;
        return allJobs;
    }

    /// Update a list of filters used to retrieve jobs. 
    /// <param name="filters">A dictionary of filter keys and values to apply when retrieving jobs.
    public void UpdateFilters(IReadOnlyDictionary<string, string>? filters)
    {
        filtersDictionary = filters?.ToDictionary(k => k.Key, v => v.Value) ?? [];
    }
}