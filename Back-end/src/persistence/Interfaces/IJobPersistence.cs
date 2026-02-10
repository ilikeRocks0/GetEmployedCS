using Back_end.Persistence.Objects;

namespace Back_end.Persistence.Interfaces;

public interface IJobPersistence
{
  public List<Job> GetJobs(List<string> language, List<string> positionType, List<string> employmentType);
  public List<Job> GetSavedJobs(int userId, string language, string positionType, string employmentType);
}