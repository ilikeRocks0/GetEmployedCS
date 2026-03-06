using Back_end.Persistence.Objects;

public interface IShuffleJobsService
{
    public List<Job> ShuffleJobs(List<Job> jobs);
}