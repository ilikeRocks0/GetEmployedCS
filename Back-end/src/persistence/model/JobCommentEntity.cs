using Microsoft.EntityFrameworkCore;

namespace Back_end.Persistence.Model;

[PrimaryKey(nameof(job_comment_id))]
public class JobCommentEntity
{
  public int job_comment_id { get; set; }
  public int job_id { get; set; }
  public int poster_id { get; set; }
  public required string comment { get; set; }
  public UserEntity? poster { get; set; }
  public JobEntity? job { get; set; }
}