using Microsoft.EntityFrameworkCore;

using Back_end.Persistence.Interfaces;
using Back_end.Persistence.Objects;
using Back_end.Persistence.Model;
using Back_end.Persistence.Implementations.Types;
using Back_end.Persistence.Implementations.Queries;
using Back_end.Persistence.Implementations.Adapters.EntityAdapters;

namespace Back_end.Persistence.Implementations;

public class JobPersistence : IJobPersistence
{
    private IConfiguration config;

    public JobPersistence(IConfiguration config)
    {
        this.config = config;
    }

    private List<JobEntity> FilterQuery(IQueryable<JobEntity> query, string searchTerm, List<string> languages, List<string> positionTypes, List<string> employmentTypes, int startIndex, int pageSize)
    {
        // Add filters to query if present
        if (!searchTerm.Equals(String.Empty))
        {
            query = query.Where(e =>
              e.job_title.Contains(searchTerm) ||
              e.poster!.employer!.employer_name.Contains(searchTerm) ||
              e.poster!.jobSeeker!.first_name.Contains(searchTerm) ||
              e.poster!.jobSeeker!.last_name.Contains(searchTerm) ||
              e.locations.Any(l =>
                l.location.city.Contains(searchTerm) ||
                l.location.state.Contains(searchTerm) ||
                l.location.country.Contains(searchTerm)
              )
            );
        }
        if (languages.Count > 0)
        {
            query = query.Where(e => e.programmingLanguages!.Any(l => languages.Contains(l.language_name)));
        }
        if (positionTypes.Count > 0)
        {
            query = query.Where(e => positionTypes.Contains(e.position_type));
        }
        if (employmentTypes.Count > 0)
        {
            query = query.Where(e => employmentTypes.Contains(e.employment_type));
        }

        // Filter number of jobs
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
            JobQuery query = new JobQuery(context.Jobs)
              .IncludeLocations()
              .IncludePoster()
              .IncludeProgrammingLanguages();

            // Execute query and get results
            List<JobEntity> jobEntities = FilterQuery(query.Query, searchTerm, languages, positionTypes, employmentTypes, startIndex, pageSize);

            // Convert entities to business objects and add to the return list
            jobEntities.ForEach(e =>
            {
                filteredList.Add(new JobEntityAdapter(e));
            });
        }

        return filteredList;
    }

    public List<Job> GetSavedJobs(int userId, string searchTerm, List<string> languages, List<string> positionTypes, List<string> employmentTypes, int startIndex, int pageSize)
    {
        List<Job> savedJobs = new();

        using (AppDbContext context = new(this.config))
        {
            JobSeekerEntity? jobSeekerEntity = new JobSeekerQuery(context.JobSeekers)
                                                .IncludeLikes()
                                                .GetJobSeekerByUserId(userId);

            // Gather liked jobs into a list
            List<JobEntity> jobEntities = new();
            jobSeekerEntity?.likes?.ToList().ForEach(e =>
            {
                jobEntities.Add(e.savedJob!);
            });

            // Run filter on query and convert each entity to business object
            FilterQuery(jobEntities.AsQueryable(), searchTerm, languages, positionTypes, employmentTypes, startIndex, pageSize)
              .ForEach(e =>
              {
                  savedJobs.Add(new JobEntityAdapter(e));
              });
        }

        return savedJobs;
    }

    public List<string> GetProgrammingLanguages()
    {
        List<string> languages = new();

        using (AppDbContext context = new(this.config))
        {
            context.ProgrammingLanguages.ToList()
              .ForEach(e => languages.Add(e.language_name));
        }

        return languages;
    }

    public int GetNumberOfJobs(string searchTerm, List<string> languages, List<string> positionTypes, List<string> employmentTypes)
    {
        return GetJobs(searchTerm, languages, positionTypes, employmentTypes, 0, int.MaxValue).Count();
    }

    public int GetNumberOfSavedJobs(int userId, string searchTerm, List<string> languages, List<string> positionTypes, List<string> employmentTypes)
    {
        return GetSavedJobs(userId, searchTerm, languages, positionTypes, employmentTypes, 0, int.MaxValue).Count();
    }

    public List<JobComment> GetJobComments(int jobId)
    {
        List<JobComment> comments = new();

        using (AppDbContext context = new(this.config))
        {
            JobEntity? job = context.Jobs.Where(e => e.job_id == jobId)
              .Include(e => e.comments!)
                .ThenInclude(e => e.poster)
                  .ThenInclude(e => e!.employer)
              .SingleOrDefault();

            // Add the retrieved jobs comments to the list that will be returned
            job?.comments?.ToList().ForEach(e =>
            {
                comments.Add(new JobComment(e.comment, e.poster_id, e.job!.job_id));
            });
        }

        return comments;
    }

    public string? CreateJobComment(JobComment comment)
    {
        string? text = null;

        using (AppDbContext context = new(this.config))
        {
            JobCommentEntity newComment = new()
            {
                job_id = comment.JobId,
                poster_id = comment.PosterUserId,
                comment = comment.Comment
            };

            context.JobComments.Add(newComment);
            context.SaveChanges();

            text = newComment.comment;
        }

        return text;
    }  

    public Job? GetJobFromJobId(int jobId)
    {
      using (AppDbContext context = new(this.config))
      {
        Job? job = null;
        JobEntity? e = new JobQuery(context.Jobs)
            .IncludeLocations()
            .GetJobByJobId(jobId);
        if (e != null)
            {
                job = new JobEntityAdapter(e);
            }
        return job;   
      }
    }
}