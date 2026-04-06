using Back_end.Persistence.Interfaces;
using Back_end.Objects;
using Back_end.Services.Interfaces;
using Back_end.Endpoints.Models;

namespace Back_end.Services.Implementations;

public class UserCommentsService(IUserPersistence userPersistence) : IUserCommentsService
{
    public List<UserComment> GetComments(string username)
    {
        if(username == null || username.Trim().Equals(string.Empty))
        {
            throw new ArgumentException("Username must be not empty");
        }
        User? user = userPersistence.GetUserByUsername(username);
        if (user == null)
        {
            throw new NullReferenceException("User not found");
        }

        return userPersistence.GetProfileComments(user.UserId);
    }

    public UserComment CreateComment(NewUserComment comment)
    {
        User? user = userPersistence.GetUserByUsername(comment.CommentedUserUsername);

        if (user == null)
        {
            throw new NullReferenceException("User not found");
        }
        else if(comment.Comment.Trim().Equals(string.Empty))
        {
            throw new ArgumentException("Comment string cannot be empty");
        }

        UserComment NewComment = new UserComment(comment.Comment, comment.PosterUserId, user.UserId, user.Username);

        return userPersistence.CreateUserComment(NewComment);
    }
}