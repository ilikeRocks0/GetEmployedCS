using Back_end.Persistence.Objects;

public class JobComment
{
    public string Comment { get; set; }
    public int PosterUserId { get; set; }
    public int JobId { get; set; }

    public string PosterUsername { get; set; }

    public JobComment(string comment, int posterId, int jobId, string posterUsername)
    {
        this.Comment = comment;
        this.PosterUserId = posterId;
        this.JobId = jobId;
        this.PosterUsername = posterUsername;
    }
}