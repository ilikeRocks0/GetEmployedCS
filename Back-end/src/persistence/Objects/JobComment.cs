using Back_end.Persistence.Objects;

public class JobComment
{
    public string Comment { get; }
    public User Poster { get; }
    public int JobId { get; }

    public JobComment(string comment, User poster, int jobId)
    {
        this.Comment = comment;
        this.Poster = poster;
        this.JobId = jobId;
    }
}