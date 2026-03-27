using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace Back_end.Persistence.Model;

[PrimaryKey(nameof(language_name), nameof(job_id))]
public class JobLanguageEntity
{
    public int job_id { get; set; }
    public required string language_name { get; set; }
    public JobEntity? job { get; set; }
    public ProgrammingLanguageEntity? language { get; set; }

    [SetsRequiredMembers]
    public JobLanguageEntity(int job_id, string language_name)
    {
        this.job_id = job_id;
        this.language_name = language_name;
    }
}