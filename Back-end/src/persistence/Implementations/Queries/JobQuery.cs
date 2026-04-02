using Back_end.Persistence.Implementations.Adapters.EntityAdapters;
using Back_end.Persistence.Model;
using Back_end.Objects;
using Microsoft.EntityFrameworkCore;

namespace Back_end.Persistence.Implementations.Queries;

public class JobQuery
{
    public IQueryable<JobEntity> Query { get; set; }

    public JobQuery(DbSet<JobEntity> jobs)
    {
        this.Query = jobs;
    }

    public JobQuery IncludeLocations()
    {
        this.Query = this.Query.Include(e => e.locations!)
                    .ThenInclude(e => e.location);

        return this;
    }

    public JobQuery IncludeProgrammingLanguages()
    {
        this.Query = this.Query.Include(e => e.programmingLanguages);

        return this;
    }

    public JobQuery IncludePoster()
    {
        this.Query = this.Query.Include(e => e.poster)
                    .ThenInclude(e => e!.employer)
                  .Include(e => e.poster)
                    .ThenInclude(e => e!.jobSeeker);

        return this;
    }

    public JobEntity? GetJobByJobId(int jobId)
    {
        return this.Query.Where(e => e.job_id == jobId).SingleOrDefault();
    }

    public List<Job> GetJobByPosterUsername(string username)
    {
        return this.Query
                    .Where(e => e.poster!.username.Equals(username))
                    .ToList()
                    .Select(e => (Job)new JobEntityAdapter(e))
                    .ToList();
    }
}