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

    //<param name="jobList">A list of jobs to extract a user's saved jobs from.</param>
    //<param name="userId">The unique identifier of the user.</param>
    public List<Job> GetJobsSavedSublist(List<Job> jobs, int userId);
   
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
    //Saves a comment to a job.
    //</summary>
    //<param name="comment">The comment metadata.</param>
    //<returns>Returns the comment.</returns>
    public JobComment CreateJobComment(JobComment comment);

    //<summary>
    //Gets a job based on the jobId. 
    //</summary>
    //<param name="jobId">The target job's jobId.</param>
    //<returns>Returns the matching Job object if found, else returns null.</returns>
    public Job? GetJobFromJobId(int jobId);

    //<summary>
    //Creates a new job given a Job object.
    //</summary>
    //<param name="job">The job to be created.</param>
    //<returns>Returns the ID of the newly created job in the database if successful, else returns -1.</returns>
    public int CreateJob(Job job);

    //<summary>
    //Gets jobs posted by a given username.
    //</summary>
    //<param name="username">The username to find posted jobs for</param>
    //<returns>Returns a list containing the jobs posted by the given username.</returns>
    public List<Job> GetJobsByUsername(string username);

    //<summary>
    //Deletes a job given a job ID
    //</summary>
    //<param name="jobId">The ID of the job to be deleted</param>
    public void DeleteJob(int jobId);
}