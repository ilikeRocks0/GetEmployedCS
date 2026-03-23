using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
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
    public ICollection<JobLanguageEntity>? programmingLanguages { get; set; }
    public ICollection<JobLocationEntity>? locations { get; set; }
    public ICollection<LikeEntity>? likes { get; set; }
    public ICollection<JobCommentEntity>? comments { get; set; }

    public JobEntity() {}

    [SetsRequiredMembers]
    public JobEntity(string job_title, DateTime? application_deadline, string application_link, bool? has_remote, bool? has_hybrid, string position_type, string employment_type, string job_description)
    {
        this.job_title = job_title;
        this.application_deadline = application_deadline;
        this.application_link = application_link;
        this.has_remote = has_remote;
        this.has_hybrid = has_hybrid;
        this.position_type = position_type;
        this.employment_type = employment_type;
        this.job_description = job_description;
    }
}
