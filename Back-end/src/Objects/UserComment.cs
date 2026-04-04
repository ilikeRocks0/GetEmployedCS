namespace Back_end.Objects;

public class UserComment
{
    public string Comment { get; set; }
    public int PosterUserId { get; set; }
    public int ProfileUserId { get; set; }
    public string PosterUsername { get; set; }

    public UserComment(string comment, int posterId, int profileUserId, string posterUsername)
    {
        this.Comment = comment;
        this.PosterUserId = posterId;
        this.ProfileUserId = profileUserId;
        this.PosterUsername = posterUsername;
    }
}