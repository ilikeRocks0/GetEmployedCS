using Back_end.Persistence.Implementations.Adapters.EntityAdapters;
using Back_end.Persistence.Implementations.Adapters.ObjectAdapters;
using Back_end.Persistence.Interfaces;
using Back_end.Persistence.Model;
using Back_end.Objects;
using Back_end.Persistence.Implementations.Queries;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Back_end.Persistence.Exceptions;

namespace Back_end.Persistence.Implementations;

public class UserPersistence : IUserPersistence
{
    private readonly IConfiguration config;
    private readonly IPasswordHasher<User> passwordHasher;

    public UserPersistence(IConfiguration config, IPasswordHasher<User> passwordHasher)
    {
        this.config = config;
        this.passwordHasher = passwordHasher;
    }

    //<summary>
    //Gets a user from a userId.
    //</summary>
    //<param name="jobId">The target user's userId.</param>
    //<returns>A matching User object if found, else returns null.</returns>
    public User? GetUser(int userId)
    {
        User? user = null;

        using (AppDbContext context = new(this.config))
        {
            // Build the query for the user in question
            UserEntity? userEntity = context.Users
              .Where(e => e.user_id == userId)
              .Include(e => e.employer)
              .Include(e => e.jobSeeker)
                .ThenInclude(e => e!.experiences)
              .SingleOrDefault();

            if (userEntity != null)
            {
                user = new UserEntityAdapter(userEntity);
            }
        }

        return user;
    }

    //<summary>
    //Gets a list of users based on the params given.
    //</summary>
    //<param name="searchTerm">A keyword string to filter users by name, username, or email.</param>
    //<param name="employer">A flag for whether to find employers (true) or not (false).</param>
    //<param name="startIndex">The index of the record in the Users table to start returning entities.</param>
    //<param name="pageSize">The max amount of objects to return.</param>
    //<returns>A list of Users.</returns>
    public List<User> GetUsers(string searchTerm, bool employer, int startIndex, int pageSize)
    {
        using(AppDbContext context = new(this.config))
        {
            string lowerSearchTerm = searchTerm.ToLower();
            List<User> users;

            if(employer)
            {
                users = context.Employers
                                    .Include(e => e.user)
                                        .ThenInclude(e => e!.employer)
                                    .Where(e => e.employer_name.ToLower().Contains(lowerSearchTerm) || e.user!.username.ToLower().Contains(lowerSearchTerm) || e.user.email.ToLower().Contains(lowerSearchTerm))
                                    .Select(e => (User)new UserEntityAdapter(e.user!))
                                    .ToList();
            }
            else
            {
                users = context.JobSeekers
                                    .Include(e => e.user)
                                        .ThenInclude(e => e!.jobSeeker)
                                    .Where(e => e.first_name.ToLower().Contains(lowerSearchTerm) || e.last_name.ToLower().Contains(lowerSearchTerm) || e.user!.username.ToLower().Contains(lowerSearchTerm) || e.user.email.ToLower().Contains(lowerSearchTerm))
                                    .Select(e => (User)new UserEntityAdapter(e.user!))
                                    .ToList();
            }

            return users;
        }
    }

    //<summary>
    //Gets a list of users for the user game (employer side of job game).
    //</summary>
    //<param name="currentUserId">A keyword string to filter users by name, username, or email.</param>
    //<param name="startIndex">The index of the record in the Users table to start returning entities.</param>
    //<param name="pageSize">The max amount of users to return.</param>
    //<returns>A list of Users.</returns>
    public List<User> GetUsersForGame(int currentUserId, int startIndex, int amount)
    {
        const int batchSize = 50;

        using (AppDbContext context = new(this.config))
        {
            var result = new List<User>(capacity: amount);
            var skip = startIndex;

            while (result.Count < amount)
            {
                var batch = context.Users
                  .Where(e => e.user_id != currentUserId && e.employer == null)
                  .OrderBy(e => e.user_id)
                  .Skip(skip)
                  .Take(batchSize)
                  .Include(e => e.employer)
                  .Include(e => e.jobSeeker)
                    .ThenInclude(e => e!.experiences)
                  .ToList();

                if (batch.Count == 0)
                {
                    break;
                }

                foreach (UserEntity entity in batch)
                {
                    if (result.Count >= amount)
                    {
                        break;
                    }

                    try
                    {
                        result.Add(new UserEntityAdapter(entity));
                    }
                    catch (ObjectConversionException)
                    {
                        // Skip rows that fail entity invariants (e.g. bad email)
                    }
                }

                skip += batch.Count;
            }

            return result;
        }
    }

    //<summary>
    //Gets a user from a username.
    //</summary>
    //<param name="username">The target user's username.</param>
    //<returns>A matching User object if found, else returns null.</returns>
    public User? GetUserByUsername(string username)
    {
        User? user = null;

        using (AppDbContext context = new(this.config))
        {
            // Build the query for the user in question
            UserEntity? userEntity = context.Users
              .Where(e => e.username == username)
              .Include(e => e.employer)
              .Include(e => e.jobSeeker)
                .ThenInclude(e => e!.experiences)
              .SingleOrDefault();

            if (userEntity != null)
            {
                user = new UserEntityAdapter(userEntity);
            }
        }

        return user;
    }

    //<summary>
    //Creates a new user based on a User object.
    //</summary>
    //<param name="newUser">The user to be created.</param>
    //<returns>The ID of the newly created user in the database.</returns>
    public (int userId, string verifyToken) CreateUser(User newUser)
    {
        using (AppDbContext context = new(this.config))
        {
            var hashedPassword = passwordHasher.HashPassword(newUser, newUser.Password);

            UserEntity newUserEntity = new()
            {
                email = newUser.Email,
                username = newUser.Username,
                password = hashedPassword,
                about_string = newUser.About,
                verified = false,
                verify_token = Guid.NewGuid().ToString()
            };

            context.Users.Add(newUserEntity);
            context.SaveChanges();

            // Determine if we need to add a corresponding employer or job seeker to the employer or job seeker tables respectively
            if (newUser.IsEmployer)
            {
                EmployerEntity newEmployerEntity = new()
                {
                    employer_name = newUser.EmployerName!,
                    user = newUserEntity
                };

                newUserEntity.employer = newEmployerEntity;
                context.Employers.Add(newEmployerEntity);
                context.SaveChanges();
            }
            else
            {
                // Need to convert the experience objects to experience entities
                List<ExperienceEntity> experienceEntities = new();
                newUser.Experiences!.ForEach(e => experienceEntities.Add(new ExperienceObjectAdapter(e)));

                JobSeekerEntity newJobSeekerEntity = new()
                {
                    first_name = newUser.FirstName!,
                    last_name = newUser.LastName!,
                    user = newUserEntity
                };

                // Add job seeker entities before its experiences to ensure FK constraints aren't violated
                newUserEntity.jobSeeker = newJobSeekerEntity;
                context.JobSeekers.Add(newJobSeekerEntity);
                context.SaveChanges();

                // Add experience entities
                context.Experiences.AddRange(experienceEntities);
                context.SaveChanges();
            }

            // Return the user ID and verify token of the newly added entity
            return (newUserEntity.user_id, newUserEntity.verify_token!);
        }
    }

    //<summary>
    //Saves a job for the user.
    //</summary>
    //<param name="userId">The id of the user to save the job for.</param>
    //<param name="jobId">The id of the job to save.</param>
    //<returns>0 if successful, else returns -1.</returns>
    public int SaveJob(int userId, int jobId)
    {
        int success = -1;

        using (AppDbContext context = new(this.config))
        {
            //Get the JobSeekerEntity matching the given user ID 
            JobSeekerEntity? jobSeekerEntity = new JobSeekerQuery(context.JobSeekers).GetJobSeekerByUserId(userId);

            // Make the entity and save it in the Likes table
            LikeEntity? newLikeEntity;
            if (jobSeekerEntity is not null)
            {
                newLikeEntity = new()
                {
                    seeker_id = jobSeekerEntity.seeker_id,
                    job_id = jobId,
                };

                context.Likes.Add(newLikeEntity);
                context.SaveChanges();
                success = 0;
            }

            return success;
        }
    }

    //<summary>
    //Unsaves a saved job for the user.
    //</summary>
    //<param name="userId">The id of the user to unsave the job for.</param>
    //<param name="jobId">The id of the job to unsave.</param>
    //<returns>True if successful, else returns false.</returns>
    public bool UnsaveJob(int userId, int jobId)
    {
        bool result = false;

        using (AppDbContext context = new(this.config))
        {
            // Query for the job seeker that is unsaving the job
            JobSeekerEntity? jobSeeker = new JobSeekerQuery(context.JobSeekers).IncludeLikes().GetJobSeekerByUserId(userId);

            if(jobSeeker is not null)
            {
                // Get the corresponding like entity
                LikeEntity? likeEntity = context.Likes.Where(e => e.seeker_id == jobSeeker.seeker_id && e.job_id == jobId).SingleOrDefault();

                // If the like entity is null, then the user has not saved this job
                if(likeEntity is not null)
                {
                    context.Likes.Remove(likeEntity);
                    context.SaveChanges();
                    result = true;
                }
            }
        }

        return result;
    }

    //<summary>
    //Checks if a job is in a user's liked (saved) jobs list.
    //</summary>
    //<param name="userId">The id of the user to check the likes list for.</param>
    //<param name="jobId">The id of the job to check for.</param>
    //<returns>True if successful, else returns false.</returns>
    public bool IsJobInLikes(int userId, int jobId)
    {
        bool isInLikes = false;

        using (AppDbContext context = new(this.config))
        {
            //Get the JobSeekerEntity matching the given user ID 
            JobSeekerEntity? jobSeekerEntity = new JobSeekerQuery(context.JobSeekers).IncludeLikes().GetJobSeekerByUserId(userId);

            if (jobSeekerEntity is not null)
            {
                //find if the job seeker has liked the specified job - isInLikes = true if so
                isInLikes = jobSeekerEntity.likes?.Any(like => like.job_id == jobId) ?? false;
            }
            return isInLikes;
        }
    }

    //<summary>
    //Gets a user if the provided credentials match theirs. 
    //</summary>
    //<param name="email">The email to find a user for.</param>
    //<param name="password">The password to find a user for.</param>
    //<returns>A matching user if successful, else returns null.</returns>
    public User? GetUserByCredentials(string email, string password)
    {
        User? user = null;

        using (AppDbContext context = new(this.config))
        {
            UserEntity? userEntity = context.Users
              .Where(e => e.email.Equals(email))
              .Include(e => e.employer)
              .Include(e => e.jobSeeker)
                .ThenInclude(e => e!.experiences)
              .SingleOrDefault();

            if (userEntity != null)
            {
                var candidateUser = new UserEntityAdapter(userEntity);
                var verificationResult = passwordHasher.VerifyHashedPassword(
                    candidateUser,
                    userEntity.password,
                    password);

                if (verificationResult == PasswordVerificationResult.Success ||
                    verificationResult == PasswordVerificationResult.SuccessRehashNeeded)
                {
                    user = candidateUser;
                }
            }
        }

        return user;
    }

    //<summary>
    //Causes a user to follow another user. 
    //</summary>
    //<param name="followerId">The id of the user following another user.</param>
    //<param name="followedId">The id of the user being followed.</param>
    public void FollowUser(int followerId, int followedId)
    {
        using(AppDbContext context = new(this.config))
        {
            context.Follows.Add(new FollowsEntity(followerId, followedId));
            context.SaveChanges();
        }
    }

    //<summary>
    //Causes a user to unfollow another user. 
    //</summary>
    //<param name="followerId">The id of the user following another user.</param>
    //<param name="followedId">The id of the user being unfollowed.</param>
    public void UnfollowUser(int followerId, int followedId)
    {
        using(AppDbContext context = new(this.config))
        {
            context.Follows.Where(e => e.follower_id == followerId && e.followed_id == followedId).ExecuteDelete();
        }
    }

    //<summary>
    //Checks if a user is following another user.
    //</summary>
    //<param name="followerId">The id of the user following another user.</param>
    //<param name="followedId">The id of the user being followed.</param>
    //<returns>True if the followerId user follows the followedId user, else returns false.</returns>
    public bool IsUserInFollows(int followerId, int followedId)
    {
        using(AppDbContext context = new(this.config))
        {
            return context.Follows.Where(e => e.follower_id == followerId && e.followed_id == followedId).SingleOrDefault() is not null;
        }
    }

    //<summary>
    //Gets a list of all the followers of a user.
    //</summary>
    //<param name="userId">The id of the user with the follows.</param>
    //<returns>A list of users.</returns>
    public List<User> GetAllFollowers(int userId)
    {
        List<User> followers = new();

        using(AppDbContext context = new(this.config))
        {
            followers = context.Follows
                                    .Where(e => e.followed_id == userId)
                                    .Include(e => e.follower)
                                        .ThenInclude(e => e!.employer)
                                    .Include(e => e.follower)
                                        .ThenInclude(e => e!.jobSeeker)
                                    .Select(e => (User)new UserEntityAdapter(e.follower!))
                                    .ToList();
        }

        return followers;
    }

    //<summary>
    //Gets a list of all the users a user is following.
    //</summary>
    //<param name="userId">The id of the user who follows other users.</param>
    //<returns>A list of users.</returns>
    public List<User> GetAllFollowing(int userId)
    {
        using(AppDbContext context = new(this.config))
        {
            return context.Follows
                                .Where(e => e.follower_id == userId)
                                .Include(e => e.followed)
                                    .ThenInclude(e => e!.employer)
                                .Include(e => e.followed)
                                    .ThenInclude(e => e!.jobSeeker)
                                .Select(e => (User)new UserEntityAdapter(e.followed!))
                                .ToList();
        }
    }

    //<summary>
    //Checks is a user is an employer. 
    //</summary>
    //<param name="userId">The id of the user to check.</param>
    //<returns>True if the user is an employer, else returns false.</returns>
    public bool CheckUserEmployer(int userId)
    {
        using(AppDbContext context = new(this.config))
        {
            return context.Employers.Where(e => e.user_id == userId).SingleOrDefault() is not null;
        }
    }

    //<summary>
    //Updates a user's attributes.
    //</summary>
    //<param name="updatedUser">A User object with the updated attributes.</param>
    public void UpdateUser(User updatedUser)
    {
        using(AppDbContext context = new(this.config))
        {
            UserEntity? userEntity = context.Users.Where(e => e.user_id == updatedUser.UserId).SingleOrDefault() ?? throw new InvalidOperationException("An existing user could not be found in database");

            // Update user fields
            userEntity.username = updatedUser.Username;
            userEntity.password = updatedUser.Password == userEntity.password
                ? userEntity.password
                : passwordHasher.HashPassword(updatedUser, updatedUser.Password);
            userEntity.about_string = updatedUser.About;
            userEntity.email = updatedUser.Email;
            
            // Update fields specific to each user type
            if(updatedUser.IsEmployer && updatedUser.EmployerName is not null)
            {
                EmployerEntity? employerEntity = context.Employers.Where(e => e.user_id == updatedUser.UserId).SingleOrDefault();
                if(employerEntity is not null)
                {
                    employerEntity.employer_name = updatedUser.EmployerName;
                }
            }
            else if(!updatedUser.IsEmployer && updatedUser.FirstName is not null && updatedUser.LastName is not null)
            {
                JobSeekerEntity? jobSeekerEntity = context.JobSeekers.Where(e => e.user_id == updatedUser.UserId).SingleOrDefault();
                if(jobSeekerEntity is not null)
                {
                    jobSeekerEntity.first_name = updatedUser.FirstName;
                    jobSeekerEntity.last_name = updatedUser.LastName;
                }
            }

            context.SaveChanges();
        }
    }

    //<summary>
    //Adds a new experience for a user.
    //</summary>
    //<param name="userId">The id of the user to add the experience to.</param>
    //<param name="experience">The experience to add.</param>
    //<returns>The ID newly added experience.</returns>
    public int CreateExperience(int userId, Experience experience)
    {
        using(AppDbContext context = new(this.config))
        {
            JobSeekerEntity? jobSeeker = new JobSeekerQuery(context.JobSeekers)
                                            .IncludeExperiences()
                                            .GetJobSeekerByUserId(userId) 
                                            ?? throw new InvalidOperationException("An existing user could not be found in database");

            ExperienceEntity experienceEntity = new ExperienceObjectAdapter(experience);
            jobSeeker.experiences!.Add(experienceEntity);
            context.SaveChanges();

            return experienceEntity.experience_id;
        }
    }

    //<summary>
    //Gets a list of experiences belonging to a user.
    //</summary>
    //<param name="userId">The id of the user to to get the experienecs of.</param>
    //<returns>A list of Experiences.</returns>
    public List<Experience> GetExperiences(int userId)
    {
        using(AppDbContext context = new(this.config))
        {
            JobSeekerEntity jobSeeker = new JobSeekerQuery(context.JobSeekers)
                                            .IncludeExperiences()
                                            .GetJobSeekerByUserId(userId)
                                            ?? throw new InvalidOperationException("An existing user could not be found in database");

            return jobSeeker.experiences!
                    .Select(e => (Experience)new ExperienceEntityAdapter(e))
                    .ToList();
        }
    }

    //<summary>
    //Updates a user's experience.
    //</summary>
    //<param name="userId">The id of the user who has the experience.</param>
    //<param name="oldExperience">The experience being updated, used to find the matching experience for the user.</param>
    //<param name="newExperience">An experience with the attributes to use to update.</param>
    public void UpdateExperience(int userId, Experience oldExperience, Experience newExperience)
    {
        using(AppDbContext context = new(this.config))
        {
            JobSeekerEntity jobSeeker = new JobSeekerQuery(context.JobSeekers)
                                            .IncludeExperiences()
                                            .GetJobSeekerByUserId(userId)
                                            ?? throw new InvalidOperationException("An existing user could not be found in database");

            ExperienceEntity experienceEntity = jobSeeker.experiences!
                                                    .AsQueryable()
                                                    .Where(e => e.company_name.Equals(oldExperience.CompanyName)
                                                        && e.position_title.Equals(oldExperience.PositionTitle)
                                                        && e.job_description.Equals(oldExperience.JobDescription))
                                                    .Single();

            ExperienceEntity newEntity = new ExperienceObjectAdapter(newExperience)
            {
                experience_id = experienceEntity.experience_id,
                seeker_id = experienceEntity.seeker_id
            };
            context.Entry(experienceEntity).CurrentValues.SetValues(newEntity);
            context.SaveChanges();
        }
    }

    //<summary>
    //Deletes a user's experience.
    //</summary>
    //<param name="userId">The id of the user who has the experience.</param>
    //<param name="experience">The experience to delete, used to find the matching experience for the user.</param>
    public void DeleteExperience(int userId, Experience experience)
    {
        using(AppDbContext context = new(this.config))
        {
            JobSeekerEntity jobSeeker = new JobSeekerQuery(context.JobSeekers)
                                            .IncludeExperiences()
                                            .GetJobSeekerByUserId(userId)
                                            ?? throw new InvalidOperationException("An existing user could not be found in database");

            ExperienceEntity experienceEntity = jobSeeker.experiences!
                                                    .AsQueryable()
                                                    .Where(e => e.company_name.Equals(experience.CompanyName)
                                                        && e.position_title.Equals(experience.PositionTitle)
                                                        && e.job_description.Equals(experience.JobDescription))
                                                    .Single();

            context.Experiences.Remove(experienceEntity);
            context.SaveChanges();
        }
    }

    //<summary>
    //Checks if the user owns the specified experience.
    //</summary>
    //<param name="userId">The id of the user to check the experience for.</param>
    //<param name="experienceId">The id of the experience to check if it belongs to the user.</param>
    //<returns>True if the user owns the experience, else return false.</returns>
    public bool IsExperienceOwner(int userId, int experienceId)
    {
        using(AppDbContext context = new(this.config))
        {
            JobSeekerEntity? jobSeeker = new JobSeekerQuery(context.JobSeekers)
                                            .IncludeExperiences()
                                            .GetJobSeekerByUserId(userId);

            return jobSeeker?.experiences?.Any(e => e.experience_id == experienceId) ?? false;
        }
    }

    //<summary>
    //Marks a user as verified.
    //</summary>
    //<param name="token">The token to find a matching user for and verify them if found.</param>
    public void VerifyUser(string token)
    {
        using(AppDbContext context = new(this.config))
        {
            UserEntity userEntity = context.Users.Where(e => e.verify_token!.Equals(token)).Single();
            userEntity.verified = true;
            userEntity.verify_token = null;
            context.SaveChanges();
        }
    }

    //<summary>
    //Add a new comment under a user profile.
    //</summary>
    //<param name="comment">The comment to add to the profile.</param>
    //<returns>The newly created UserComment.</returns>
    public UserComment CreateUserComment(UserComment comment)
    {
        using (AppDbContext context = new(this.config))
        {
            UserCommentEntity newComment = new()
            {
                profile_id = comment.ProfileUserId,
                poster_id = comment.PosterUserId,
                comment = comment.Comment
            };

            context.UserComments.Add(newComment);
            context.SaveChanges();
        }

        return comment;
    }

    //<summary>
    //Gets a list of comments on a user profile.
    //</summary>
    //<param name="userId">The id of the user to get profile comments for.</param>
    //<returns>A list of UserComments.</returns>
    public List<UserComment> GetProfileComments(int userId)
    {
        List<UserComment> comments = new();

        using(AppDbContext context = new(this.config))
        {
            UserEntity user = context.Users
                            .Where(e => e.user_id == userId)
                            .Include(e => e.profileComments!)
                                .ThenInclude(e => e.posterUser)
                            .Single();

            if(user.profileComments is not null)
            {
                comments = user.profileComments
                                .Where(e => e.posterUser is not null)
                                .Select(e => (UserComment)new UserCommentEntityAdapter(e)).ToList();
            }
        }

        return comments;
    }
}