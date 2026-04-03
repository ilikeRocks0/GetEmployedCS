using Back_end.Objects;

namespace Back_end.Services.Interfaces;

public interface IJobIndexManager
{
    public List<Job> GetJobs();

    public void UpdateFilters(IReadOnlyDictionary<string, string>? filters);
}