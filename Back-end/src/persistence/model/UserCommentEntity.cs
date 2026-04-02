using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace Back_end.Persistence.Model;

[PrimaryKey(nameof(user_comment_id))]
public class UserCommentEntity
{
    public int user_comment_id { get; set; }
    public int profile_id { get; set; }
    public int poster_id { get; set; }
    public required string comment { get; set; }
    public UserEntity? profileUser { get; set; }
    public UserEntity? posterUser { get; set; }

}