using Back_end.Persistence.Interfaces;
using Back_end.Persistence.Objects;
using Back_end.Util;

namespace Back_end.Services.Interfaces;

class CommentsService (IJobPersistence jobPersistence, IUserPersistence userPersistence) : ICommentsService
{
    public List<JobComment> GetComments(int jobId)
    {
        return jobPersistence.GetJobComments(jobId);
    }

    public JobComment CreateComment(NewJobComment comment)
    {
        User? user = userPersistence.GetUser(comment.PosterUserId);
        if (user == null)
        {
            throw new Exception("User not found");
        }
        JobComment NewComment = new JobComment(comment.Comment, comment.PosterUserId, comment.JobId, user.Username);

        return jobPersistence.CreateJobComment(NewComment);
    }
}