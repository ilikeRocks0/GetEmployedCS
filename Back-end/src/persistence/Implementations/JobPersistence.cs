using Microsoft.EntityFrameworkCore;

using Back_end.Persistence.Interfaces;
using Back_end.Persistence.Objects;
using Back_end.Persistence.Model;
using Back_end.Persistence.Implementations.Types;

namespace Back_end.Persistence.Implementations;

public class JobPersistence : IJobPersistence
{
  private IConfiguration config;

  public JobPersistence(IConfiguration config)
  {
    this.config = config;
  }

  private List<string> LanguageEntitiesToStrings(ICollection<JobLanguageEntity>? entities)
  {
    List<string> strings = new List<string>();

    entities?.ToList().ForEach(e => strings.Add(e.language_name));

    return strings;
  }

  private Job JobEntityToObject(JobEntity e)
  {
    // Convert DateTime from job entity to a DateOnly
    ApplicationDate deadline = new(e.application_deadline);

    // Create Poster object that validates and constructs the poster's name 
    Poster poster = new(e.poster, e.employer_poster);

    // Get job locations
    List<string> locations = new();
    List<JobLocationEntity> jobLocationEntities = e.locations.ToList();

    jobLocationEntities.ForEach(e =>
    {
      locations.Add(new JobLocation(e.location).Location);
    });

    // Create and return new object
    return new Job(
      e.job_title,
      deadline.Date,
      poster.Name,
      e.application_link,
      e.has_remote,
      e.has_hybrid,
      e.position_type,
      e.employment_type,
      locations,
      LanguageEntitiesToStrings(e.programmingLanguages),
      e.job_description
    );
  }

  private List<JobEntity> FilterQuery(IQueryable<JobEntity> query, string searchTerm, List<string> languages, List<string> positionTypes, List<string> employmentTypes, int startIndex, int pageSize)
  {
    // Add filters to query if present
    if(!searchTerm.Equals(String.Empty))
    {
      query = query.Where(e => e.job_title.Contains(searchTerm));
    }
    if(languages.Count > 0)
    {
      query = query.Where(e => languages.Intersect(LanguageEntitiesToStrings(e.programmingLanguages)).Any());
    }
    if(positionTypes.Count > 0)
    {
      query = query.Where(e => positionTypes.Contains(e.position_type));
    }
    if(employmentTypes.Count > 0)
    {
      query = query.Where(e => employmentTypes.Contains(e.employment_type));
    }

    query.OrderBy(e => e.job_id)
      .Skip(startIndex)
      .Take(pageSize);

    return query.ToList();
  }

  public List<Job> GetJobs(string searchTerm, List<string> languages, List<string> positionTypes, List<string> employmentTypes, int startIndex, int pageSize)
  {
    List<Job> filteredList = new();

    // Create AppDbContext instance to open short-term DB connection
    using (AppDbContext context = new(this.config))
    {
      // Build initial query
      // Will get all jobs if no filters are passed
      var query = context.Jobs
        .Include(e => e.locations)
          .ThenInclude(e => e.location)
        .Include(e => e.programmingLanguages)
        .AsQueryable();

      // Execute query and get results
      List<JobEntity> jobEntities = FilterQuery(query, searchTerm, languages, positionTypes, employmentTypes, startIndex, pageSize);

      // Convert entities to business objects and add to the return list
      jobEntities.ForEach(e =>
      {
        filteredList.Add(JobEntityToObject(e));
      });
    }

    return filteredList;
  }

  public List<Job> GetSavedJobs(int userId, string searchTerm, List<string> languages, List<string> positionTypes, List<string> employmentTypes, int startIndex, int pageSize)
  {
    List<Job> savedJobs = new();

    using (AppDbContext context = new(this.config))
    {
      JobSeekerEntity? jobSeekerEntity = context.JobSeekers
        .Where(e => e.user_id == userId)
        .Include(e => e.likes!)
          .ThenInclude(e => e.savedJob)
            .ThenInclude(e => e.programmingLanguages)
        .Include(e => e.likes!)
          .ThenInclude(e => e.savedJob)
            .ThenInclude(e => e.locations)
              .ThenInclude(e => e.location)
        .SingleOrDefault();

      // Gather liked jobs into a list
      List<JobEntity> jobEntities = new();
      jobSeekerEntity?.likes?.ToList().ForEach(e =>
      {
        jobEntities.Add(e.savedJob);
      });

      // Run filter on query and convert each entity to business object
      FilterQuery(jobEntities.AsQueryable(), searchTerm, languages, positionTypes, employmentTypes, startIndex, pageSize)
        .ForEach(e =>
        {
          savedJobs.Add(JobEntityToObject(e));
        });
    }

    return savedJobs;
  }

  public int GetNumberOfJobs(string searchTerm, List<string> languages, List<string> positionTypes, List<string> employmentTypes)
  {
    return GetJobs(searchTerm, languages, positionTypes, employmentTypes, 0, int.MaxValue).Count();
  }

  public int GetNumberOfSavedJobs(int userId, string searchTerm, List<string> languages, List<string> positionTypes, List<string> employmentTypes)
  {
    return GetSavedJobs(userId, searchTerm, languages, positionTypes, employmentTypes, 0, int.MaxValue).Count();
  }
}