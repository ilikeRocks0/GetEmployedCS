using Back_end.Objects;

namespace Back_end.Services.Interfaces;

public interface IShuffleJobsService
{
    public List<Job> ShuffleJobs(List<Job> jobs);
}