using Microsoft.EntityFrameworkCore;

namespace Back_end.Persistence.Model;

[PrimaryKey(nameof(follower_id), nameof(followed_id))]
public class FollowsEntity
{
    public int follower_id { get; set; }
    public int followed_id { get; set; }
    public UserEntity? follower;
    public UserEntity? followed;

    public FollowsEntity(int follower_id, int followed_id)
    {
        this.follower_id = follower_id;
        this.followed_id = followed_id;
    }
}