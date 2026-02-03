using Microsoft.EntityFrameworkCore;

namespace Back_end.Persistence.Model;

[PrimaryKey(nameof(employer_comment_id))]
public class EmployerCommentEntity
{
    public int employer_comment_id { get; set; }
    public int? seeker_id { get; set; }
    public int employer_id { get; set; }
    public required string comment { get; set; }
}
