using Back_end.Persistence.Objects;

public class JobComment
{
    public string Comment { get; }
    public int PosterUserId { get; }
    public int JobId { get; }

    public JobComment(string comment, int posterId, int jobId)
    {
        this.Comment = comment;
        this.PosterUserId = posterId;
        this.JobId = jobId;
    }
}