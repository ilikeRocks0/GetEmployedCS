using Back_end.Persistence.Implementations.Types;
using Back_end.Persistence.Model;
using Back_end.Objects;
using Back_end.Persistence.Implementations.Validation;
using Back_end.Persistence.Exceptions;

namespace Back_end.Persistence.Implementations.Adapters.EntityAdapters;

public class JobEntityAdapter : Job
{
    private void ValidateEntity(JobEntity jobEntity)
    {
        if (jobEntity.job_title.Trim().Equals(String.Empty))
        {
            throw new ObjectConversionException("Job entity cannot have empty job title.");
        }

        if (!ValidationRegex.linkRegex.IsMatch(jobEntity.application_link))
        {
            throw new ObjectConversionException("Job entity must have a valid application link.");
        }

        if (jobEntity.position_type.Trim().Equals(String.Empty))
        {
            throw new ObjectConversionException("Job entity cannot have empty position type.");
        }

        if (jobEntity.employment_type.Trim().Equals(String.Empty))
        {
            throw new ObjectConversionException("Job entity cannot have empty employment type.");
        }

        if (jobEntity.job_description.Trim().Equals(String.Empty))
        {
            throw new ObjectConversionException("Job entity cannot have empty job description.");
        }
    }

    public JobEntityAdapter(JobEntity jobEntity) : base(jobEntity.job_title, jobEntity.application_link, jobEntity.position_type, jobEntity.employment_type, jobEntity.job_description)
    {
        ValidateEntity(jobEntity);

        JobId = jobEntity.job_id;

        // Convert DateTime from job entity to a DateOnly
        ApplicationDeadline = new ApplicationDate(jobEntity.application_deadline).Date;

        // Create Poster object that validates and constructs the poster's name 
        PosterName = new Poster(jobEntity.poster, jobEntity.employer_poster).ToString();

        // Set employer poster flag
        EmployerPoster = jobEntity.employer_poster;

        // Get job locations
        Locations = new();
        List<JobLocationEntity> jobLocationEntities = jobEntity.locations!.ToList();

        jobLocationEntities.ForEach(e =>
        {
            Locations.Add(new JobLocation(e.location!).Location);
        });

        ProgrammingLanguages = new List<string>();
        jobEntity.programmingLanguages?.ToList().ForEach(e => ProgrammingLanguages.Add(e.language_name));
    }
}