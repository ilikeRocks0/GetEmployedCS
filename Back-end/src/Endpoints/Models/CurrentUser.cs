namespace Back_end.Endpoints.Models;
public class CurrentUser
{
    public int UserId { get; set; }

    public CurrentUser(int userId)
  {
    this.UserId = userId;
  }
}