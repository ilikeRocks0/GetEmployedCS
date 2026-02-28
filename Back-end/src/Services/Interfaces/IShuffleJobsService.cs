using Back_end.Persistence.Objects;

interface IShuffleJobsService
{
    public List<Job> ShuffleJobs(List<Job> jobs);
}