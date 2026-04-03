using Back_end.Persistence.Model;
using Back_end.Objects;
using Back_end.Persistence.Exceptions;

namespace Back_end.Persistence.Implementations.Adapters.EntityAdapters;

public class ExperienceEntityAdapter : Experience
{
    private void ValidateEntity(ExperienceEntity experienceEntity)
    {
        if (experienceEntity.company_name.Trim().Equals(String.Empty))
        {
            throw new ObjectConversionException("Experience entity cannot have empty company name.");
        }

        if (experienceEntity.position_title.Trim().Equals(String.Empty))
        {
            throw new ObjectConversionException("Experience entity cannot have empty position title.");
        }
    }

    public ExperienceEntityAdapter(ExperienceEntity experienceEntity) : base(experienceEntity.experience_id, experienceEntity.company_name, experienceEntity.position_title, experienceEntity.job_description)
    {
        ValidateEntity(experienceEntity);
    }
}