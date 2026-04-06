using Back_end.Persistence.Interfaces;
using Back_end.Objects;
using Back_end.Services.Interfaces;
using Back_end.Endpoints.Models;
namespace Back_end.Services.Implementations;

public class CommentsService (IJobPersistence jobPersistence, IUserPersistence userPersistence) : ICommentsService
{
    /// Get a list of comments for a job. 
    /// <param name="jobId">An id corresponding to a job to get the comments of
    /// Returns a list of comments, if there are any and if the jobId corresponds to a valid job.
    public List<JobComment> GetComments(int jobId)
    {
        if(jobId < 0)
        {
            throw new ArgumentException("Job ID of comment must be non-negative");
        }

        return jobPersistence.GetJobComments(jobId);
    }

    /// Add a new comment under a job. 
    /// <param name="comment">A temporary newJobComment holding attributes necessary to add the comment under a job.
    /// - This is converted to a User object with a helper function. 
    /// Returns the newly created JobComment. 
    public JobComment CreateComment(NewJobComment comment)
    {
        User? user = userPersistence.GetUser(comment.PosterUserId);
        
        if (user == null)
        {
            throw new NullReferenceException("User not found");
        }
        else if(comment.JobId < 0)
        {
            throw new ArgumentException("Job ID of comment must be non-negative");
        }
        else if(comment.Comment.Trim().Equals(String.Empty))
        {
            throw new ArgumentException("Comment string cannot be empty");
        }

        JobComment NewComment = new JobComment(comment.Comment, comment.PosterUserId, comment.JobId, user.Username);

        return jobPersistence.CreateJobComment(NewComment);
    }
}