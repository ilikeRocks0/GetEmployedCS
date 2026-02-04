using Back_end.Persistance;

namespace Back_end.Services;

public interface IJobService
{
    IReadOnlyList<Job> GetJobs(IReadOnlyDictionary<string, string>? filters = null);
    IReadOnlyList<Job> GetSavedJobs(IReadOnlyDictionary<string, string>? filters = null);
}
