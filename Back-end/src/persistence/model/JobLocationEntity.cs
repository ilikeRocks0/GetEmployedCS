
using Back_end.Persistence.Model;
using Microsoft.EntityFrameworkCore;

[PrimaryKey(nameof(job_id), nameof(location_id))]
public class JobLocationEntity
{
  public int job_id { get; set; }
  public int location_id { get; set; }
  public required JobEntity job { get; set; }
  public required LocationEntity location { get; set; }
}