using Back_end.Persistence.Objects;

public interface IJobIndexManager
{
    public List<Job> GetJobs();

    public void UpdateFilters(IReadOnlyDictionary<string, string>? filters);
}