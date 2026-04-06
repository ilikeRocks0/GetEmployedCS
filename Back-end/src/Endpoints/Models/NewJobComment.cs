namespace Back_end.Endpoints.Models;

public class NewJobComment
{
    public string Comment { get; set; }
    public int PosterUserId { get; set; }
    public int JobId { get; set; }

    public NewJobComment(string comment, int posterUserId, int jobId)
    {
        this.Comment = comment;
        this.PosterUserId = posterUserId;
        this.JobId = jobId;
    }
}