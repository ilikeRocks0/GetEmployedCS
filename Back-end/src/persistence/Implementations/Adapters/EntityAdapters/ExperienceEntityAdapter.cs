using Back_end.Persistence.Model;
using Back_end.Persistence.Objects;

namespace Back_end.Persistence.Implementations.Adapters.EntityAdapters;

public class ExperienceEntityAdapter : Experience
{
  public ExperienceEntityAdapter(ExperienceEntity experienceEntity): base(experienceEntity.company_name, experienceEntity.position_title, experienceEntity.job_description)
  {
  }
}