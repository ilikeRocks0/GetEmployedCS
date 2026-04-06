using Back_end.Objects;

namespace Back_end.Services.Interfaces;

public interface IJobIndexManager
{
    /// Get a list of jobs. 
    /// Returns a list of all jobs using existing filtration.
    public List<Job> GetJobs();

    /// Update a list of filters used to retrieve jobs. 
    /// <param name="filters">A dictionary of filter keys and values to apply when retrieving jobs.
    public void UpdateFilters(IReadOnlyDictionary<string, string>? filters);
}