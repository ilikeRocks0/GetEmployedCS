using Back_end.Endpoints.Models;
using Back_end.Objects;

namespace Back_end.Services.Interfaces;

public interface ICommentsService
{
    /// Get a list of comments for a job. 
    /// <param name="jobId">An id corresponding to a job to get the comments of
    /// Returns a list of comments, if there are any and if the jobId corresponds to a valid job.
    public List<JobComment> GetComments(int jobId);

    /// Add a new comment under a job. 
    /// <param name="comment">A temporary newJobComment holding attributes necessary to add the comment under a job.
    /// Returns the newly created JobComment. 
    public JobComment CreateComment(NewJobComment comment);
}