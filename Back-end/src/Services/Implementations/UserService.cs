using Back_end.Endpoints.Models;
using Back_end.Persistence.Interfaces;
using Back_end.Objects;
using Back_end.Services.Implementations.Finders;
using Back_end.Services.Interfaces;
using Back_end.Util;

public class UserService(IUserPersistence userPersistence, IJobPersistence jobPersistence, IEmailService emailService) : IUserService
{
    public List<User> GetAllFollowing(int userId) => userPersistence.GetAllFollowing(userId);
    public List<User> GetAllFollowers(int userId) => userPersistence.GetAllFollowers(userId);

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

    public async Task<int> CreateUser(NewUser newUser)
    {
        User savedUser = ExtractUserFromInput(newUser);
        var (userId, verifyToken) = userPersistence.CreateUser(savedUser);
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

    public int Login(LoginRequest loginRequest)
    {
        var user = userPersistence.GetUserByCredentials(loginRequest.Email.ToLower(), loginRequest.Password);
        if (user is null || !user.Verified)
            throw new InvalidOperationException("Invalid username or password.");
        return user.UserId;
    }

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

    public bool CheckUserEmployer(int userId)
    {
        return userPersistence.CheckUserEmployer(userId);
    }

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

    public int AddExperience(int userId, Experience experience)
    {
        if (userPersistence.CheckUserEmployer(userId))
        {
            throw new InvalidOperationException("Only job seekers can add experiences.");
        }


        return userPersistence.CreateExperience(userId, experience);
    }

    public void EditExperience(int userId, Experience oldExperience, Experience newExperience)
    {
        if (userPersistence.CheckUserEmployer(userId))
            throw new InvalidOperationException("Only job seekers can edit experiences.");

        userPersistence.UpdateExperience(userId, oldExperience, newExperience);
    }

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