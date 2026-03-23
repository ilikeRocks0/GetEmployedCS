using System.Diagnostics.CodeAnalysis;
using Back_end.Persistence.Model;
using Back_end.Persistence.Objects;

namespace Back_end.Persistence.Implementations.Adapters.ObjectAdapters;

//Converts a internal object to an Entity object for the database. 
public class ExperienceObjectAdapter : ExperienceEntity
{
    [SetsRequiredMembers]
    public ExperienceObjectAdapter(Experience experience) : base(experience.CompanyName, experience.PositionTitle, experience.JobDescription)
    {
        ValidateObject(experience);
    }

    private void ValidateObject(Experience experience)
    {
        if(experience.CompanyName.Trim().Equals(String.Empty))
        {
            throw new ObjectConversionException("Experience cannot have empty company name.");
        }

        if(experience.PositionTitle.Trim().Equals(String.Empty))
        {
            throw new ObjectConversionException("Experience cannot have empty position title.");
        }
    }
}