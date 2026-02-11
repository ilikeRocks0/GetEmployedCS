using Back_end.Persistence.Model;
using Microsoft.EntityFrameworkCore;

[PrimaryKey(nameof(user_id))]
public class UserEntity
{
  public int user_id { get; set; }
  public required string username { get; set; }
  public required string password { get; set; }
  public string? about_string { get; set; }
  public ICollection<JobEntity>? postedJobs { get; set; }
  public EmployerEntity? employer { get; set; }
  public JobSeekerEntity? jobSeeker {get; set; }
}
