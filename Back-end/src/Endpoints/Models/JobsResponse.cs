using Back_end.Persistence.Objects;

namespace Back_end.Endpoints.Models;
public class JobsResponse
{
    public IReadOnlyList<Job> JobList { get; set; }
    public IReadOnlyList<int> SavedJobIds { get; set; }
    
    public JobsResponse(IReadOnlyList<Job> jobList, IReadOnlyList<int> savedJobIds)
  {
    this.JobList = jobList;
    this.SavedJobIds = savedJobIds;
  }
}