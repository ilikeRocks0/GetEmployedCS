namespace Back_end.Services.Interfaces;
interface ICommentsService
{
    public List<JobComment> GetComments(int jobId);
    public JobComment CreateComment(NewJobComment comment);
}