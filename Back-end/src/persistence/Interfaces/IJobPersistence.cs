using Back_end.Persistence.Objects;

namespace Back_end.Persistence.Interfaces;

public interface IJobPersistence
{
    //<param name="searchTerm">A keyword string to filter jobs by title.</param>
    //<param name="languages">A list of required programming languages or technologies.</param>
    //<param name="positionTypes">The roles to filter by (e.g., "front-end", "back-end").</param>
    //<param name="employmentTypes">The types of contract to filter by (e.g., "full-time", "part-time).</param>
  public List<Job> GetJobs(string searchTerm, List<string> languages, List<string> positionTypes, List<string> employmentTypes);
  
    //<param name="seekerId">The unique identifier of the user.</param>
    //<param name="searchTerm">A keyword string to filter jobs by title.</param>
    //<param name="languages">A list of required programming languages or technologies.</param>
    //<param name="positionTypes">The roles to filter by (e.g., "front-end", "back-end").</param>
    //<param name="employmentTypes">The types of contract to filter by (e.g., "full-time", "part-time).</param>
  public List<Job> GetSavedJobs(int seekerId, string searchTerm, List<string> languages, List<string> positionTypes, List<string> employmentTypes);
}