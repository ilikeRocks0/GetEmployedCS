using Back_end.Persistence.Objects;

namespace Back_end.Persistence.Interfaces;

public interface IJobPersistence
{
    //<param name="searchTerm">A keyword string to filter jobs by title.</param>
    //<param name="languages">A list of required programming languages or technologies.</param>
    //<param name="positionTypes">The roles to filter by (e.g., "front-end", "back-end").</param>
    //<param name="employmentTypes">The types of contract to filter by (e.g., "full-time", "part-time).</param>
    //<param name="startIndex">The index of the record in the Jobs table to start returning entities.</param>
    //<param name="pageSize">The max amount of objects to return.</param>
  public List<Job> GetJobs(string searchTerm, List<string> languages, List<string> positionTypes, List<string> employmentTypes, int startIndex, int pageSize);
  
    //<param name="userId">The unique identifier of the user.</param>
    //<param name="searchTerm">A keyword string to filter jobs by title.</param>
    //<param name="languages">A list of required programming languages or technologies.</param>
    //<param name="positionTypes">The roles to filter by (e.g., "front-end", "back-end").</param>
    //<param name="employmentTypes">The types of contract to filter by (e.g., "full-time", "part-time).</param>
    //<param name="startIndex">The index of the record in the Jobs table to start returning entities.</param>
    //<param name="pageSize">The max amount of objects to return.</param>
  public List<Job> GetSavedJobs(int userId, string searchTerm, List<string> languages, List<string> positionTypes, List<string> employmentTypes, int startIndex, int pageSize);

    //<summary>
    //Returns a list of programming languages for the front-end.
    //</summary>
  public List<string> GetProgrammingLanguages();

    //<param name="searchTerm">A keyword string to filter jobs by title.</param>
    //<param name="languages">A list of required programming languages or technologies.</param>
    //<param name="positionTypes">The roles to filter by (e.g., "front-end", "back-end").</param>
    //<param name="employmentTypes">The types of contract to filter by (e.g., "full-time", "part-time).</param>
  public int GetNumberOfJobs(string searchTerm, List<string> languages, List<string> positionTypes, List<string> employmentTypes);
  
    //<param name="userId">The unique identifier of the user.</param>
    //<param name="searchTerm">A keyword string to filter jobs by title.</param>
    //<param name="languages">A list of required programming languages or technologies.</param>
    //<param name="positionTypes">The roles to filter by (e.g., "front-end", "back-end").</param>
    //<param name="employmentTypes">The types of contract to filter by (e.g., "full-time", "part-time).</param>
  public int GetNumberOfSavedJobs(int userId, string searchTerm, List<string> languages, List<string> positionTypes, List<string> employmentTypes);

    //<summary>
    //Gets the comments from a job for a given job id.
    //</summary>
    //<param name="jobId">The job id that we will get comments for.</param>
    //<returns>Returns a list of comments.</returns>
  public List<JobComment> GetJobComments(int jobId);

    //<summary>
    //Saves a comment to the a job.
    //</summary>
    //<param name="comment">The comment metadata.</param>
    //<returns>Returns the id of a job.</returns>
  public int CreateJobComment(JobComment comment);
}