using System.Text.RegularExpressions;
using Back_end.Persistence.Implementations.Types;
using Back_end.Persistence.Model;
using Back_end.Persistence.Objects;

public class JobEntityAdapter : Job
{
    private void ValidateEntity(JobEntity jobEntity)
    {
        Regex linkRegex = new Regex("(https://)?(\\S+)\\.(\\S+\\.)*(\\S+)(/\\S+)");

        if(jobEntity.job_title.Trim().Equals(String.Empty))
        {
            throw new ObjectConversionException("Job entity cannot have empty job title.");
        }

        if(!linkRegex.IsMatch(jobEntity.application_link))
        {
            throw new ObjectConversionException("Job entity must have a valid application link.");
        }

        if(jobEntity.position_type.Trim().Equals(String.Empty))
        {
            throw new ObjectConversionException("Job entity cannot have empty position type.");
        }

        if(jobEntity.employment_type.Trim().Equals(String.Empty))
        {
            throw new ObjectConversionException("Job entity cannot have empty employment type.");
        }

        if(jobEntity.job_description.Trim().Equals(String.Empty))
        {
            throw new ObjectConversionException("Job entity cannot have empty job description.");
        }
    }

    public JobEntityAdapter(JobEntity jobEntity) : base (jobEntity.job_title, jobEntity.application_link, jobEntity.position_type, jobEntity.employment_type, jobEntity.job_description)
    {
        ValidateEntity(jobEntity);

          // Convert DateTime from job entity to a DateOnly
        ApplicationDate deadline = new(jobEntity.application_deadline);

        // Create Poster object that validates and constructs the poster's name 
        Poster poster = new(jobEntity.poster, jobEntity.employer_poster);

        // Get job locations
        List<string> locations = new();
        List<JobLocationEntity> jobLocationEntities = jobEntity.locations.ToList();

        jobLocationEntities.ForEach(e =>
        {
            locations.Add(new JobLocation(e.location).Location);
        });

        List<string> languages = new List<string>();
        jobEntity.programmingLanguages?.ToList().ForEach(e => languages.Add(e.language_name));

        ApplicationDeadline = deadline.Date;
        PosterName = poster.Name;
        Locations = locations;
        ProgrammingLanguages = languages;
    }
} 