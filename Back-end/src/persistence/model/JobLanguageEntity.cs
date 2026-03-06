using Microsoft.EntityFrameworkCore;

namespace Back_end.Persistence.Model;

[PrimaryKey(nameof(language_name), nameof(job_id))]
public class JobLanguageEntity
{
    public int job_id { get; set; }
    public required string language_name { get; set; }
    public required JobEntity job { get; set; }
    public required ProgrammingLanguageEntity language { get; set; }
}