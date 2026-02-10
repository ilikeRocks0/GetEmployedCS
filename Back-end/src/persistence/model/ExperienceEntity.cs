using Microsoft.EntityFrameworkCore;

namespace Back_end.Persistence.Model;

[PrimaryKey(nameof(experience_id))]
public class ExperienceEntity
{
    public int experience_id { get; set; }
    public int seeker_id { get; set; }
    public required string company_time { get; set; }
    public required string position_title { get; set; }
    public string? job_description { get; set; }
}
