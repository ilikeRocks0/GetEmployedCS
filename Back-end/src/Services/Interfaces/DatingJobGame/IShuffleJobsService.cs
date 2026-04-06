using Back_end.Objects;

namespace Back_end.Services.Interfaces;

public interface IShuffleJobsService
{
    /// Get a shuffled version of a list of jobs. 
    /// <param name="jobs">The list of jobs to shuffle.
    /// Returns a shuffled version of the jobs parameter.
    public List<Job> ShuffleJobs(List<Job> jobs);
}