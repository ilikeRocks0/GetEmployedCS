using Microsoft.EntityFrameworkCore;

namespace Back_end.Persistence.Model;

[PrimaryKey(nameof(job_id), nameof(folder_id))]
public class FolderContentEntity
{
  public int job_id { get; set; }
  public int folder_id { get; set; }
}