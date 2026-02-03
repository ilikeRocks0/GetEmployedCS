using Back_end.Persistence.Objects;

namespace Back_end.Services.Interfaces;

public interface IJobService
{
    IReadOnlyList<Job> GetJobs(IReadOnlyDictionary<string, string>? filters = null);
    IReadOnlyList<Job> GetSavedJobs(IReadOnlyDictionary<string, string>? filters = null);
}
