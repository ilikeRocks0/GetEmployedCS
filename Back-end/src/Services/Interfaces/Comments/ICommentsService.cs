using Back_end.Endpoints.Models;
using Back_end.Objects;

namespace Back_end.Services.Interfaces;

public interface ICommentsService
{
    public List<JobComment> GetComments(int jobId);
    public JobComment CreateComment(NewJobComment comment);
}