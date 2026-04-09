using Back_end.Endpoints.Models;
using Back_end.Persistence.Interfaces;
using Back_end.Objects;
using Back_end.Services.Implementations.Finders;
using Back_end.Services.Interfaces;
using Back_end.Util;
using Microsoft.EntityFrameworkCore;

public class UserService(IUserPersistence userPersistence, IJobPersistence jobPersistence, IEmailService emailService) : IUserService
{
    /// Get a list of all the users a user is following. 
    /// <param name="userId">An id corresponding to the user to get a following list for.
    /// Returns a list of Users if the userId belongs to a valid User.
    public List<User> GetAllFollowing(int userId) => userPersistence.GetAllFollowing(userId);
    public List<User> GetAllFollowers(int userId) => userPersistence.GetAllFollowers(userId);

    /// Get a list of users. 
    /// <param name="filters">A dictionary of filter keys and values to apply when retrieving users. Supported keys include:
    /// - "SearchTerm": A keyword string to filter users by.
    /// - "PageNumber" and "StartIndex: Where to skip to in order to begin in the list of users, if not already at the start.
    /// If null or empty, no filters will be applied and all users will be returned.
    public List<User> GetUsers(IReadOnlyDictionary<string, string>? filters = null)
    {
        var searchTerm = filters?.GetValueOrDefault(AppConfig.FilterKeys.SEARCH_TERM) ?? "";
        var pageNumber = int.TryParse(filters?.GetValueOrDefault(AppConfig.FilterKeys.PAGE_NUMBER), out var pg) ? pg : 1;
        var startIndex = (pageNumber - 1) * AppConfig.ITEMS_PER_PAGE;

        if (filters != null && bool.TryParse(filters.GetValueOrDefault(AppConfig.FilterKeys.EMPLOYER), out var employer))
        {
            return userPersistence.GetUsers(searchTerm, employer, startIndex, AppConfig.ITEMS_PER_PAGE);
        }

        // No employer filter — return both employers and job seekers
        var employers = userPersistence.GetUsers(searchTerm, true, startIndex, AppConfig.ITEMS_PER_PAGE);
        var seekers = userPersistence.GetUsers(searchTerm, false, startIndex, AppConfig.ITEMS_PER_PAGE);
        return [..employers, ..seekers];
    }

    /// Add a new user. 
    /// <param name="newUser">A temporary newUser holding user attributes passed in through the front end
    /// - This is converted to a User object with a helper function. 
    /// Returns the new user's userId. 
    public async Task<int> CreateUser(NewUser newUser)
    {
        if (userPersistence.GetUserByUsername(newUser.Username) != null)
            throw new InvalidOperationException("Username is already taken.");

        User savedUser = ExtractUserFromInput(newUser);
        int userId;
        string verifyToken;
        try
        {
            (userId, verifyToken) = userPersistence.CreateUser(savedUser);
        }
        catch (DbUpdateException ex)
        {
            var inner = ex.InnerException?.Message ?? ex.Message;
            if (inner.Contains("Duplicate entry") && inner.Contains("email"))
                throw new InvalidOperationException("Email is already taken.");
            throw;
        }
        try
        {
            await emailService.SendVerificationEmailAsync(newUser.Email, verifyToken);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[CreateUser] Failed to send verification email to {newUser.Email}: {ex.Message}");
        }
        return userId;
    }
    
    /// Saves a job to a user's saved list. 
    /// <param name="filters">A dictionary of filter keys and values to apply when retrieving the user and jobs. Supported keys include:
    /// - "userId": An id to find a matching user for.
    /// - "jobId":An id to find a matching job for.
    /// Must ensure the job is not already saved for the user. 
    /// Returns 0 if successful, -1 if unsuccessful.
    public int SaveJob(IReadOnlyDictionary<string, string>? filters = null)
    {
        if (filters == null)
        {
            throw new InvalidOperationException("Invalid filter parameters");
        }
        var userId = int.TryParse(filters.GetValueOrDefault(AppConfig.FilterKeys.USERID), out var uId) ? uId : 0;
        var jobId = int.TryParse(filters.GetValueOrDefault(AppConfig.FilterKeys.JOBID), out var jId) ? jId : 0;

        if (userId == 0)
        {
            throw new InvalidOperationException("Invalid user id.");
        }

        if (jobId == 0)
        {
            throw new InvalidOperationException("Invalid job id.");
        }



        if (userPersistence.IsJobInLikes(userId, jobId))
        {
            throw new InvalidOperationException("This job has already been liked by this user");
        }
        return userPersistence.SaveJob(userId, jobId);
    }

    /// Unsaves (removes) a job from a user's saved list. 
    /// <param name="filters">A dictionary of filter keys and values to apply when retrieving the user and jobs. Supported keys include:
    /// - "userId": An id to find a matching user for.
    /// - "jobId":An id to find a matching job for.
    /// Must ensure the job is initially saved for the user. 
    /// Returns true if successful, false if unsuccessful.
    public bool UnsaveJob(IReadOnlyDictionary<string, string>? filters = null)
    {
        if (filters == null)
        {
            throw new InvalidOperationException("Invalid filter parameters");
        }
        var userId = int.TryParse(filters.GetValueOrDefault(AppConfig.FilterKeys.USERID), out var uId) ? uId : 0;
        var jobId = int.TryParse(filters.GetValueOrDefault(AppConfig.FilterKeys.JOBID), out var jId) ? jId : 0;
        
        if (!userPersistence.IsJobInLikes(userId, jobId))
        {
            throw new InvalidOperationException("This job has not already been liked by this user");
        }
        return userPersistence.UnsaveJob(userId, jobId);
    }

    /// Logs a user into their account. 
    /// <param name="loginRequest">An object containing the user's entered login credentials, including email and password. 
    /// Returns the user's userId. 
    public int Login(LoginRequest loginRequest)
    {
        var user = userPersistence.GetUserByCredentials(loginRequest.Email.ToLower(), loginRequest.Password);
        if (user is null || !user.Verified)
            throw new InvalidOperationException("Invalid username or password.");
        return user.UserId;
    }

    /// Converts a given newUser object into a User object. 
    /// <param name="newUser">A temporary newUser holding user attributes passed in through the front end
    /// - This is converted to an employer User or a job seeker User depending on the newUser's contents. 
    /// Returns the newly made User object. 
    private static User ExtractUserFromInput(NewUser newUser)
    {
        var email = newUser.Email.ToLower();
        User savedUser;
        if (newUser.IsEmployer)
        {
            savedUser = new User(
                -1,
                email,
                newUser.Username,
                newUser.Password, 
                "", 
                newUser.EmployerName);
        }
        else
        {
            savedUser = new User(
                -1,
                email,
                newUser.Username,
                newUser.Password,
                "",
                newUser.FirstName,
                newUser.LastName, 
                []);
        }
        
        return savedUser;
    }

    /// Get a profile for a user. 
    /// <param name="userId">An id corresponding to the user to get a profile for.
    /// A user is found based on the userId and is used to retrieve the profile.
    /// Returns a Profile if the userId belongs to a valid User.
    public Profile? GetProfile(int userId)
    {
        UserFinder userFinder = new UserFinder(userPersistence);
        User? user = userFinder.GetUser(userId);
        if (user == null)
        {
            throw new NullReferenceException("User not found.");   
        }
        var postedJobs = jobPersistence.GetJobsByUsername(user.Username);
        return new Profile(user.UserId, user.Username, user.Email, user.FirstName, user.LastName, user.About, user.Experiences, user.IsEmployer, user.EmployerName, postedJobs: postedJobs);
    }

    /// Get a profile for a user. 
    /// <param name="username">A username corresponding to the user to get a profile for.
    /// A user is found based on the username and is used to retrieve the profile.
    /// Returns a Profile if the username belongs to a valid User.
    public Profile? GetProfileByUsername(string username)
    {
        UserFinder userFinder = new UserFinder(userPersistence);
        User? user = userFinder.GetUserByUsername(username);
        if (user == null)
        {
            throw new NullReferenceException("User not found.");
        }
        var postedJobs = jobPersistence.GetJobsByUsername(user.Username);
        return new Profile(user.UserId, user.Username, user.Email, user.FirstName, user.LastName, user.About, user.Experiences, user.IsEmployer, user.EmployerName, postedJobs: postedJobs);
    }

    /// Checks if a user is an employer user. 
    /// <param name="userId">An id corresponding to the user to check.
    /// Returns true if the user is an employer, false if the user is a job seeker. 
    public bool CheckUserEmployer(int userId)
    {
        return userPersistence.CheckUserEmployer(userId);
    }


    /// Updates the profile information of a user. 
    /// <param name="request">An object containing the updated user attributes, including email, username, password, about and name.
    /// <param name="userId">An id corresponding to the user to update.
    /// Returns the user's updated profile after saving the update. 
    public Profile UpdateUser(UpdateUserRequest request, int userId)
    {
        UserFinder userFinder = new UserFinder(userPersistence);
        User existing = userFinder.GetUser(userId) ?? throw new InvalidOperationException("User not found.");

        User updatedUser;
        if (existing.IsEmployer)
        {
            updatedUser = new User(userId, existing.Email, request.Username ?? existing.Username, existing.Password, request.About ?? "", request.EmployerName ?? existing.EmployerName ?? "");
        }
        else
        {
            updatedUser = new User(userId, existing.Email, request.Username ?? existing.Username, existing.Password, request.About ?? "", request.FirstName ?? existing.FirstName ?? "", request.LastName ?? existing.LastName ?? "", existing.Experiences ?? []);
        }

        userPersistence.UpdateUser(updatedUser);
        return GetProfile(userId)!;
    }

    /// Add a new experience for a user. 
    /// <param name="userId">An id corresponding to the user to add the experience to.
    /// <param name="experience">The experience to add.
    /// Returns the newly added experience's id. 
    public int AddExperience(int userId, Experience experience)
    {
        if (userPersistence.CheckUserEmployer(userId))
        {
            throw new InvalidOperationException("Only job seekers can add experiences.");
        }

        return userPersistence.CreateExperience(userId, experience);
    }

    /// Updates an existing experience of a user. 
    /// <param name="userId">An id corresponding to the user to edit the experience of.
    /// <param name="oldExperience">The experience to update.
    /// <param name="newExperience">Ann experience containing the attributes to update to include. 
    public void EditExperience(int userId, Experience oldExperience, Experience newExperience)
    {
        if (userPersistence.CheckUserEmployer(userId))
            throw new InvalidOperationException("Only job seekers can edit experiences.");

        userPersistence.UpdateExperience(userId, oldExperience, newExperience);
    }

    /// Delete's an experience of a user. 
    /// <param name="userId">An id corresponding to the user to edit the experience of.
    /// <param name="experience">The experience to delete.
    public void DeleteExperience(int userId, Experience experience)
    {
        if (userPersistence.CheckUserEmployer(userId))
            throw new InvalidOperationException("Only job seekers can delete experiences.");

        userPersistence.DeleteExperience(userId, experience);
    }

    public void VerifyUser(string token)
    {
        userPersistence.VerifyUser(token);
    }
}