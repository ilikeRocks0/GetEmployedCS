using Microsoft.EntityFrameworkCore;

using Back_end.Persistence.Interfaces;
using Back_end.Objects;
using Back_end.Persistence.Model;
using Back_end.Persistence.Implementations.Queries;
using Back_end.Persistence.Implementations.Adapters.ObjectAdapters;
using Back_end.Persistence.Implementations.Types;
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
    if (!string.IsNullOrWhiteSpace(searchTerm))
    {
        var term = searchTerm.ToLower();
        query = query.Where(e =>
            e.job_title.ToLower().Contains(term) ||
            (e.poster!.employer != null && e.poster!.employer.employer_name.ToLower().Contains(term)) ||
            (e.poster!.jobSeeker != null && e.poster!.jobSeeker.first_name.ToLower().Contains(term)) ||
            (e.poster!.jobSeeker != null && e.poster!.jobSeeker.last_name.ToLower().Contains(term)) ||
            e.locations!.Any(l =>
                l.location!.city.ToLower().Contains(term) ||
                l.location.state.ToLower().Contains(term) ||
                l.location.country.ToLower().Contains(term)
            )
        );
    }

    if (languages.Any())
    {
        var lowerLangs = languages.Select(l => l.ToLower()).ToList();
        query = query.Where(e => e.programmingLanguages!.Any(l => lowerLangs.Contains(l.language_name.ToLower())));
    }
    
    if (positionTypes.Any())
    {
        var lowerPos = positionTypes.Select(p => p.ToLower()).ToList();
        query = query.Where(e => lowerPos.Contains(e.position_type.ToLower()));
    }
    
    if (employmentTypes.Any())
    {
        var lowerEmp = employmentTypes.Select(e => e.ToLower()).ToList();
        query = query.Where(e => lowerEmp.Contains(e.employment_type.ToLower()));
    }

    // Filter number of jobs
    query = query.OrderBy(e => e.job_id)
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

    public List<Job> GetJobsSavedSublist(List<Job> jobs, int userId)
    {
        List<Job> savedSublist = new();

        using (AppDbContext context = new(this.config))
        {
            //Get the job seeker matching the provided userId
            JobSeekerEntity? jobSeekerEntity = new JobSeekerQuery(context.JobSeekers)
                                                .IncludeLikes()
                                                .GetJobSeekerByUserId(userId);
            ICollection<LikeEntity>? likes = jobSeekerEntity?.likes?.ToList();

            //Compare provided jobs list with the job seeker's likes
            if (likes != null)
            {
                jobs.ForEach(job =>
                {
                    if (likes.Any(likeEntry => job.JobId == likeEntry.job_id))
                    {
                        savedSublist.Add(job);
                    }
                });
            }
        }
        return savedSublist;
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
                comments.Add(new JobComment(e.comment, e.poster_id, e.job!.job_id, e.poster!.username));
            });
        }

        return comments;
    }

    public JobComment CreateJobComment(JobComment comment)
    {

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

        }

        return comment;
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

    public int CreateJob(Job job)
    {
        using (AppDbContext context = new(this.config))
        {
            // Adapt info from passed job to a partial entity before filling in navigations
            JobEntity newJobEntity = new JobObjectAdapter(job);

            // Set the poster
            if(job.PosterName is not null)
            {
                // Try to get ID from an employer
                EmployerEntity? employer = context.Employers.Where(e => e.employer_name.Equals(job.PosterName)).SingleOrDefault();
                if(employer is not null)
                {
                    newJobEntity.poster_id = employer.user_id;
                }
                // If no matching employer, try to get it from a job seeker
                else
                {
                    // Create a poster object to split the name into first and last
                    // Poster ID is set to default value 0 if no matching employer/job seeker is found as the poster
                    Poster poster = new Poster(job.PosterName, false);
                    newJobEntity.poster_id = context.JobSeekers.Where(e => e.first_name.ToLower().Equals(poster.FullName!.FirstName.ToLower()) 
                                                        && e.last_name.ToLower().Equals(poster.FullName.LastName.ToLower()))
                                                    .SingleOrDefault()?
                                                    .user_id ?? 0;
                }
            }

            // Save job entity before setting navigations
            context.Jobs.Add(newJobEntity);
            context.SaveChanges();

            // Set locations
            newJobEntity.locations = new List<JobLocationEntity>();
            job.Locations?.ForEach(locationString =>
            {
                LocationEntity locationEntity = new LocationStringAdapter(locationString);
                LocationEntity? matchingLocationEntity = context.Locations.Where(e => e.country.ToLower().Equals(locationEntity.country.ToLower()) 
                                                                && e.state.ToLower().Equals(locationEntity.state.ToLower()) 
                                                                && e.city.ToLower().Equals(locationEntity.city.ToLower()))
                                                        .SingleOrDefault();

                int locationEntityId;

                // Save location if not existing in the database already
                if(matchingLocationEntity is null)
                {
                    context.Locations.Add(locationEntity);
                    context.SaveChanges();
                    locationEntityId = locationEntity.location_id;
                }
                else
                {
                    locationEntityId = matchingLocationEntity.location_id;
                }

                // Add to join table
                JobLocationEntity jobLocationEntity = new JobLocationEntity(newJobEntity.job_id, locationEntityId);
                context.JobLocations.Add(jobLocationEntity);
            });

            // Set programming languages
            newJobEntity.programmingLanguages = new List<JobLanguageEntity>();
            job.ProgrammingLanguages?.ForEach(languageString =>
            {
                ProgrammingLanguageEntity? languageEntity = context.ProgrammingLanguages.Where(e => e.language_name.ToLower().Equals(languageString.ToLower())).SingleOrDefault();

                // Save language if not existing in the database already
                if(languageEntity is null)
                {
                    languageEntity = new ProgrammingLanguageEntity(languageString);
                    context.ProgrammingLanguages.Add(languageEntity);
                    context.SaveChanges();
                }

                // Add to join table
                JobLanguageEntity jobLanguageEntity = new JobLanguageEntity(newJobEntity.job_id, languageEntity.language_name);
                context.JobLanguages.Add(jobLanguageEntity);
            });

            // Save changes to populate join tables in database
            context.SaveChanges();

            // Return ID of new job
            return newJobEntity.job_id;
        }
    }

    public List<Job> GetJobsByUsername(string username)
    {
        using(AppDbContext context = new(this.config))
        {
            return new JobQuery(context.Jobs)
                        .IncludeLocations()
                        .IncludePoster()
                        .IncludeProgrammingLanguages()
                        .GetJobByPosterUsername(username);
        }
    }

    public void DeleteJob(int jobId)
    {
        using(AppDbContext context = new(this.config))
        {
            JobEntity? jobEntity = new JobQuery(context.Jobs).GetJobByJobId(jobId) ?? throw new InvalidOperationException("Existing job not found in the database");

            context.Jobs.Remove(jobEntity);
            context.SaveChanges();
        }
    }

    public bool IsJobOwner(int userId, int jobId)
    {
        using(AppDbContext context = new(this.config))
        {
            JobEntity? jobEntity = new JobQuery(context.Jobs).GetJobByJobId(jobId);
            return jobEntity?.poster_id == userId;
        }
    }
}