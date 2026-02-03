using Microsoft.EntityFrameworkCore;

using Back_end.Persistence.Interfaces;
using Back_end.Persistence.Objects;
using Back_end.Persistence.Model;
using Back_end.Persistence.Implementations.Types;

namespace Back_end.Persistence.Implementations;

public class JobPersistence : IJobPersistence
{
  IConfiguration config;

  public JobPersistence(IConfiguration config)
  {
    this.config = config;
  }

  private Job jobEntityToObject(JobEntity e)
  {
    // Convert DateTime from job entity to a DateOnly
    DateOnly? deadline = (e.application_deadline == null) ? null : DateOnly.FromDateTime((DateTime)e.application_deadline);

    // Create Poster object that validates and constructs the poster's name 
    Poster poster = new(e.poster, e.employer_poster);

    // Get job locations
    List<string> locations = new();
    List<JobLocationEntity> jobLocationEntities = e.locations.ToList();

    jobLocationEntities.ForEach(e =>
    {
      locations.Add((new JobLocation(e.location)).Location);
    });

    // Create and return new object
    return new Job(
      e.job_title,
      deadline,
      poster.Name,
      e.application_link,
      e.has_remote,
      e.has_hybrid,
      e.position_type,
      e.employment_type,
      locations,
      e.programming_language,
      e.job_description
    );
  }

  public List<Job> GetJobs(List<string> languages, List<string> positionTypes, List<string> employmentTypes)
  {
    List<Job> filteredList = new();

    // Create AppDbContext instance to open short-term DB connection
    using (AppDbContext context = new(this.config))
    {
      // Build initial query
      // Will get all jobs if no filters are passed
      var query = context.Jobs.AsQueryable();

      // Add filters to query if present
      if(languages.Count > 0)
      {
        query = query.Where(e => languages.Contains(e.programming_language));
      }
      if(positionTypes.Count > 0)
      {
        query = query.Where(e => positionTypes.Contains(e.position_type));
      }
      if(employmentTypes.Count > 0)
      {
        query = query.Where(e => employmentTypes.Contains(e.employment_type));
      }

      // Execute query and get results
      List<JobEntity> jobEntities = query.Include(e => e.locations).ToList();

      // Convert entities to business objects and add to the return list
      jobEntities.ForEach(e =>
      {
        filteredList.Add(jobEntityToObject(e));
      });
    }

    return filteredList;
  }

  public List<Job> GetSavedJobs(int seekerId, string language, string positionType, string employmentType)
  {
    List<Job> savedJobs = new();

    using (AppDbContext context = new(this.config))
    {
      JobSeekerEntity jobSeekerEntity = context.JobSeekers
        .Where(e => e.seeker_id == seekerId)
        .Include(e => e.likes)
        .ThenInclude(e => e.savedJob)
        .ThenInclude(e => e.locations)
        .Single();

      List<LikeEntity> likeEntities = jobSeekerEntity.likes.ToList();

      likeEntities.ForEach(e =>
      {
        savedJobs.Add(jobEntityToObject(e.savedJob));
      });
    }

    return savedJobs;
  }
}