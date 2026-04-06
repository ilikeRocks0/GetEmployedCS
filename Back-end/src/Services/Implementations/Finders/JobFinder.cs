using Back_end.Persistence.Interfaces;
using Back_end.Objects;

namespace Back_end.Services.Implementations.Finders;

public class JobFinder (IJobPersistence jobPersistence)
{    
    /// Retrieves a job based on a jobId, if there is one.
    /// <param name="jobId">An id for a job.
    /// Returns the job corresponding to the jobId. 
    public Job? GetJob(int jobId)
    {
        return jobPersistence.GetJobFromJobId(jobId);
    }
}