using Microsoft.EntityFrameworkCore;

namespace Back_end.Persistence.Model;

[PrimaryKey(nameof(language_name))]
public class ProgrammingLanguageEntity
{
  public required string language_name { get; set; }
  public ICollection<JobLanguageEntity>? jobs { get; set; }
}