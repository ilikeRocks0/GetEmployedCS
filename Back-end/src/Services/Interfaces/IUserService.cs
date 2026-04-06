using Back_end.Endpoints.Models;
using Back_end.Objects;

namespace Back_end.Services.Interfaces;

public interface IUserService
{
    /// Add a new user. 
    /// <param name="newUser">A temporary newUser holding user attributes passed in through the front end
    /// Returns the new user's userId. 
    Task<int> CreateUser(NewUser newUser);

    /// Get a list of users. 
    /// <param name="filters">A dictionary of filter keys and values to apply when retrieving users. Supported keys include:
    /// - "SearchTerm": A keyword string to filter users by.
    /// - "PageNumber" and "StartIndex: Where to skip to in order to begin in the list of users, if not already at the start.
    /// If null or empty, no filters will be applied and all users will be returned.
    List<User> GetUsers(IReadOnlyDictionary<string, string>? filters = null);

    /// Get a list of all the users a user is following. 
    /// <param name="userId">An id corresponding to the user to get a following list for.
    /// Returns a list of Users if the userId belongs to a valid User.
    List<User> GetAllFollowing(int userId);

    /// Get a profile for a user. 
    /// <param name="userId">An id corresponding to the user to get a profile for.
    /// Returns a Profile if the userId belongs to a valid User.
    Profile? GetProfile(int userId);

    /// Get a profile for a user. 
    /// <param name="username">A username corresponding to the user to get a profile for.
    /// Returns a Profile if the username belongs to a valid User.
    Profile? GetProfileByUsername(string username);

    /// Saves a job to a user's saved list. 
    /// <param name="filters">A dictionary of filter keys and values to apply when retrieving the user and jobs. Supported keys include:
    /// - "userId": An id to find a matching user for.
    /// - "jobId":An id to find a matching job for.
    /// Returns 0 if successful, -1 if unsuccessful.
    int SaveJob(IReadOnlyDictionary<string, string>? filters = null);

    /// Unsaves (removes) a job from a user's saved list. 
    /// <param name="filters">A dictionary of filter keys and values to apply when retrieving the user and jobs. Supported keys include:
    /// - "userId": An id to find a matching user for.
    /// - "jobId":An id to find a matching job for.
    /// Returns true if successful, false if unsuccessful.
    bool UnsaveJob(IReadOnlyDictionary<string, string>? filters = null);

    /// Logs a user into their account. 
    /// <param name="loginRequest">An object containing the user's entered login credentials. 
    /// Returns the user's userId. 
    int Login(LoginRequest loginRequest);

    /// Checks if a user is an employer user. 
    /// <param name="userId">An id corresponding to the user to check.
    /// Returns true if the user is an employer, false if the user is a job seeker. 
    bool CheckUserEmployer(int userId);

    /// Updates the profile information of a user. 
    /// <param name="request">An object containing the updated user attributes.
    /// <param name="userId">An id corresponding to the user to update.
    /// Returns the user's updated profile after saving the update. 
    Profile UpdateUser(UpdateUserRequest request, int userId);

    /// Add a new experience for a user. 
    /// <param name="userId">An id corresponding to the user to add the experience to.
    /// <param name="experience">The experience to add.
    /// Returns the newly added experience's id. 
    int AddExperience(int userId, Experience experience);

    /// Updates an existing experience of a user. 
    /// <param name="userId">An id corresponding to the user to edit the experience of.
    /// <param name="oldExperience">The experience to update.
    /// <param name="newExperience">Ann experience containing the attributes to update to include. 
    void EditExperience(int userId, Experience oldExperience, Experience newExperience);

    /// Delete's an experience of a user. 
    /// <param name="userId">An id corresponding to the user to edit the experience of.
    /// <param name="experience">The experience to delete.
    void DeleteExperience(int userId, Experience experience);
    void VerifyUser(string token);
    List<User> GetAllFollowers(int userId);
}