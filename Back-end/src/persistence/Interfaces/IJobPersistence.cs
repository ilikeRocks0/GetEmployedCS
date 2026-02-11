using Back_end.Persistence.Objects;

namespace Back_end.Persistence.Interfaces;

public interface IJobPersistence
{
  public List<Job> GetJobs(string searchTerm, List<string> languages, List<string> positionTypes, List<string> employmentTypes);
  public List<Job> GetSavedJobs(int seekerId, string searchTerm, List<string> languages, List<string> positionTypes, List<string> employmentTypes);
}