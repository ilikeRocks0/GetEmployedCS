namespace Back_end.Services.Interfaces;
public interface ICommentsService
{
    public List<JobComment> GetComments(int jobId);
    public JobComment CreateComment(NewJobComment comment);
}