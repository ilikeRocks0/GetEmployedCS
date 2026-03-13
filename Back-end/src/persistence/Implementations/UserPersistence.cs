using Back_end.Persistence.Implementations.Adapters.EntityAdapters;
using Back_end.Persistence.Implementations.Adapters.ObjectAdapters;
using Back_end.Persistence.Interfaces;
using Back_end.Persistence.Model;
using Back_end.Persistence.Objects;
using Back_end.Persistence.Implementations.Queries;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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

    public int CreateUser(User newUser)
    {
        int userId = -1;

        using (AppDbContext context = new(this.config))
        {
            var hashedPassword = passwordHasher.HashPassword(newUser, newUser.Password);

            UserEntity newUserEntity = new()
            {
                email = newUser.Email,
                username = newUser.Username,
                password = hashedPassword,
                about_string = newUser.About
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

            // Get the user ID of the newly added entity and return it
            userId = newUserEntity.user_id;
        }

        return userId;
    }

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

    public bool IsJobInLikes(int userId, int jobId)
    {
        bool isInLikes = false;

            using (AppDbContext context = new(this.config))
        {
            //Get the JobSeekerEntity matching the given user ID 
            JobSeekerEntity? jobSeekerEntity = new JobSeekerQuery(context.JobSeekers).GetJobSeekerByUserId(userId);

            if (jobSeekerEntity is not null)
            {
                //find if the job seeker has liked the specified job - isInLikes = true if so
                isInLikes = jobSeekerEntity.likes?.Any(like => like.job_id == jobId) ?? false;
            }
            return isInLikes;
        }
    }

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
}