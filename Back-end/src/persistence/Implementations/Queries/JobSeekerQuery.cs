using Back_end.Persistence.Model;
using Microsoft.EntityFrameworkCore;

namespace Back_end.Persistence.Implementations.Queries;

public class JobSeekerQuery
{ 
  public IQueryable<JobSeekerEntity> Query { get; }

  public JobSeekerQuery(DbSet<JobSeekerEntity> jobSeekerEntities)
  {
    this.Query = jobSeekerEntities;
  }

  public JobSeekerQuery IncludeLikes()
  {
    this.Query.Include(e => e.likes!)
                .ThenInclude(e => e.savedJob)
                  .ThenInclude(e => e.programmingLanguages)
              .Include(e => e.likes!)
                .ThenInclude(e => e.savedJob)
                  .ThenInclude(e => e.locations)
                    .ThenInclude(e => e.location);

    return this;
  }

  public JobSeekerEntity? GetJobSeekerByUserId(int userId)
  {
    return this.Query.Where(e => e.user_id == userId).SingleOrDefault();
  }
}