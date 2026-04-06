using Back_end.Objects;
using Back_end.Endpoints.Models;

namespace Back_end.Services.Interfaces;
public interface IUserCommentsService
{
    /// Get a list of comments for a user. 
    /// <param name="username">An username for the user to get comments for.
    /// Returns a list of comments, if there are any and if the username corresponds to a valid job.
    public List<UserComment> GetComments(string username);
    
    /// Add a new comment under a user profile. 
    /// <param name="comment">A temporary newUserComment holding attributes necessary to add the comment under a job.
    /// Returns the newly created UserComment. 
    public UserComment CreateComment(NewUserComment comment);
}