using Back_end.Persistence.Interfaces;
using Back_end.Persistence.Objects;
namespace Back_end.Services.Implementations.Finders;

public class JobFinder (IJobPersistence jobPersistence)
{    public Job? GetJob(int jobId)
    {
        return jobPersistence.GetJobFromJobId(jobId);
    }
}