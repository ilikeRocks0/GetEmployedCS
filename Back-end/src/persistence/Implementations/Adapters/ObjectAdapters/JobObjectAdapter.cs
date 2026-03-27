using System.Diagnostics.CodeAnalysis;
using Back_end.Persistence.Model;
using Back_end.Persistence.Objects;
using Back_end.Persistence.Implementations.Validation;

namespace Back_end.Persistence.Implementations.Adapters.ObjectAdapters;

public class JobObjectAdapter : JobEntity
{
    [SetsRequiredMembers]
    public JobObjectAdapter(Job job) : base(job.JobTitle, job.EmployerPoster, job.ApplicationDeadline?.ToDateTime(new TimeOnly()), job.ApplicationLink, job.HasRemote, job.HasHybrid, job.PositionType, job.EmploymentType, job.JobDescription)
    {
        ValidateObject(job);
    }

    private void ValidateObject(Job job)
    {
        if(job.JobTitle.Trim().Equals(String.Empty))
        {
            throw new ObjectConversionException("Job cannot have empty title.");
        }

        if(!ValidationRegex.linkRegex.IsMatch(job.ApplicationLink))
        {
            throw new ObjectConversionException("Job must have a valid application link.");
        }

        if (job.PositionType.Trim().Equals(String.Empty))
        {
            throw new ObjectConversionException("Job entity cannot have empty position type.");
        }

        if (job.EmploymentType.Trim().Equals(String.Empty))
        {
            throw new ObjectConversionException("Job cannot have empty employment type.");
        }

        if (job.JobDescription.Trim().Equals(String.Empty))
        {
            throw new ObjectConversionException("Job cannot have empty job description.");
        }
    }
}
