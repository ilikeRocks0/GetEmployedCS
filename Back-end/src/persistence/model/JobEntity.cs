using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Back_end.Persistence.Model;

[PrimaryKey(nameof(job_id))]
public class JobEntity
{
  public int job_id { get; set; }
  public int? poster_id { get; set; }
  public UserEntity? poster { get; set; }
  public bool employer_poster { get; set; }
  public required string job_title { get; set; }
  [DataType(DataType.Date)]
  [Column(TypeName = "Date")]
  public DateTime? application_deadline { get; set; }
  public int? employer_id { get; set; }
  public required string application_link { get; set; }
  public bool? has_remote { get; set; }
  public bool? has_hybrid { get; set; }
  public required string position_type { get; set; }
  public required string employment_type { get; set; }
  public required string job_description { get; set; }
  public required string programming_language { get; set; }
  public required ICollection<JobLocationEntity> locations { get; set; }
  public required ICollection<LikeEntity> likes { get; set; }
}
