using Back_end.Persistence.Interfaces;
using Back_end.Persistence.Objects;
using Back_end.Services.Interfaces;
using Back_end.Util;

namespace Back_end.Services.Implementations;

public class CommentsService (IJobPersistence jobPersistence, IUserPersistence userPersistence) : ICommentsService
{
    public List<JobComment> GetComments(int jobId)
    {
        if(jobId < 0)
        {
            throw new ArgumentException("Job ID of comment must be non-negative");
        }

        return jobPersistence.GetJobComments(jobId);
    }

    public JobComment CreateComment(NewJobComment comment)
    {
        User? user = userPersistence.GetUser(comment.PosterUserId);
        
        if (user == null)
        {
            throw new Exception("User not found");
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