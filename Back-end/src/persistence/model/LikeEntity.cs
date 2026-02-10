using Microsoft.EntityFrameworkCore;

namespace Back_end.Persistence.Model;

[PrimaryKey(nameof(seeker_id), nameof(job_id))]
public class LikeEntity
{
    public int seeker_id { get; set; }
    public int job_id { get; set; }
    public required JobEntity savedJob { get; set; }
    public required JobSeekerEntity jobSaver { get; set; }
}
