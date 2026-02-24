using Back_end.Persistence.Objects;

interface IJobIndexManager
{
    public List<Job> GetJobs();

    public void UpdateJobsList(IReadOnlyDictionary<string, string>? filters);
}