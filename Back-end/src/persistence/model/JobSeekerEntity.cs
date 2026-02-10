using Microsoft.EntityFrameworkCore;

namespace Back_end.Persistence.Model;

[PrimaryKey(nameof(seeker_id))]
public class JobSeekerEntity
{
    public int seeker_id { get; set; }
    public int user_id { get; set; }
    public required string first_name { get; set; }
    public required string last_name { get; set; }
    public required ICollection<LikeEntity> likes { get; set; }
}
