using Back_end.Persistence.Interfaces;
using Back_end.Persistence.Objects;
using Back_end.Util;

namespace Back_end.Services.Interfaces;

class CommentsService (IJobPersistence jobPersistence) : ICommentsService
{
    public List<JobComment> GetComments(int jobId)
    {
        return jobPersistence.GetJobComments(jobId);
    }

    public JobComment CreateComment(NewJobComment comment)
    {
        JobComment NewComment = new JobComment(comment.Comment, comment.PosterUserId, comment.JobId);

        return jobPersistence.CreateJobComment(NewComment);
    }
}