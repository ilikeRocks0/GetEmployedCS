using Microsoft.EntityFrameworkCore;

namespace Back_end.Persistence.Model;

[PrimaryKey(nameof(seeker_comment_id))]
public class JobSeekerCommentEntity
{
    public int seeker_comment_id { get; set; }
    public int? seeker_commenter_id { get; set; }
    public int seeker_profile_id { get; set; }
    public required string comment { get; set; }
}
