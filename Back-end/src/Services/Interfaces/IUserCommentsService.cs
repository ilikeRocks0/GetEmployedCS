using Back_end.Objects;
using Back_end.Endpoints.Models;

namespace Back_end.Services.Interfaces;
public interface IUserCommentsService
{
    // For a given user, get all comments that have been made on their profile
    public List<UserComment> GetComments(string username);
    // Create a comment on a user's profile
    public UserComment CreateComment(NewUserComment comment);
}