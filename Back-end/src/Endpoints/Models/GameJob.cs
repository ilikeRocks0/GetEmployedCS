namespace Back_end.Endpoints.Models;
public class GameJob
{
    public int UserId { get; set; }
    public int JobId { get; set; }
    
    public GameJob(int userId, int jobId)
  {
    this.UserId = userId;
    this.JobId = jobId;
  }
}