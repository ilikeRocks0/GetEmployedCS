namespace Back_end.Endpoints.Models;
public class NewUserComment
{
    public string Comment { get; set; }
    public int PosterUserId { get; set; }
    public string CommentedUserUsername { get; set; }

    public NewUserComment(string comment, int posterUserId, string commentedUserUsername)
    {
        this.Comment = comment;
        this.PosterUserId = posterUserId;
        this.CommentedUserUsername = commentedUserUsername;
    }
}