namespace Back_end.Endpoints.Models;
public class GameJob
{
    public bool IsAccepted { get; set; }
    public int UserId { get; set; }
    public int JobId { get; set; }
    
    public GameJob(bool isAccepted, int userId, int jobId)
  {
    this.IsAccepted = isAccepted;
    this.UserId = userId;
    this.JobId = jobId;
  }
}