using Back_end.Persistence.Interfaces;
using Back_end.Persistence.Model;
using Back_end.Persistence.Objects;
using Microsoft.EntityFrameworkCore;

namespace Back_end.Persistence.Implementations;

public class UserPersistence : IUserPersistence
{
  private IConfiguration config;

  public UserPersistence(IConfiguration config)
  {
    this.config = config;
  }

  private List<Experience> ExperienceEntitiesToObjects(List<ExperienceEntity> experienceEntities)
  {
    List<Experience> experiences = new List<Experience>();

    experienceEntities.ForEach(e =>
    {
      experiences.Add(new Experience(
        e.company_name,
        e.position_title,
        e.job_description
      ));
    });

    return experiences;
  }

  private List<ExperienceEntity> ExperienceObjectsToEntities(List<Experience> experiences)
  {
    List<ExperienceEntity> experienceEntities = new List<ExperienceEntity>();

    experiences.ForEach(e =>
    {
      ExperienceEntity entity = new()
      {
        company_name = e.CompanyName,
        position_title = e.PositionTitle,
        job_description = e.JobDescription
      };

      experienceEntities.Add(entity);
    });

    return experienceEntities;
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

      if(userEntity?.jobSeeker != null)
      {
        // Convert the experience entities to logic objects
        List<Experience> experiences = ExperienceEntitiesToObjects(userEntity.jobSeeker.experiences!.ToList());

        // Create new job seeker user object
        user = new User(
          userId,
          userEntity.email,
          userEntity.username,
          userEntity.password,
          userEntity.about_string,
          userEntity.jobSeeker.first_name,
          userEntity.jobSeeker.last_name,
          experiences
        );
      }
      else if(userEntity?.employer != null)
      {
        // Create new employer user object
        user = new User(
          userId,
          userEntity.email,
          userEntity.username,
          userEntity.password,
          userEntity.about_string,
          userEntity.employer.employer_name
        );
      }
    }

    return user;
  }

  public int CreateUser(User newUser)
  {
    int userId = -1;

    using (AppDbContext context = new(this.config))
    {
      // Add the user entity to the users table
      UserEntity newUserEntity = new()
      {
        email = newUser.Email,
        username = newUser.Username,
        password = newUser.Password,
        about_string = newUser.About
      };

      context.Users.Add(newUserEntity);
      context.SaveChanges();
    }

    return userId;
  }

  public int CreateJobSeeker(User newUser)
  {
    int jobSeekerId = -1;

    using (AppDbContext context = new(this.config))
    {
      // Get the matching user entity
      UserEntity newUserEntity = context.Users
        .Where(e => e.user_id == newUser.UserId)
        .Single();

      // Need to convert the experience objects to experience entities
      List<ExperienceEntity> experienceEntities = ExperienceObjectsToEntities(newUser.Experiences!);

      JobSeekerEntity newJobSeekerEntity = new()
      {
        first_name = newUser.FirstName!,
        last_name = newUser.LastName!
      };

      // Add job seeker entities before its experiences to ensure FK constraints aren't violated
      newUserEntity.jobSeeker = newJobSeekerEntity;
      context.JobSeekers.Add(newJobSeekerEntity);
      context.SaveChanges();

      // Add experience entities
      context.Experiences.AddRange(experienceEntities);
      context.SaveChanges();

      jobSeekerId = newUserEntity.jobSeeker.seeker_id;
    }

    return jobSeekerId;
  }

  public int CreateEmployer(User newUser)
  {
    int employerId = -1;
    
    using (AppDbContext context = new(this.config))
    {
      // Get the matching user entity
      UserEntity newUserEntity = context.Users
        .Where(e => e.user_id == newUser.UserId)
        .Single();

      EmployerEntity newEmployerEntity = new()
      {
        employer_name = newUser.EmployerName!,
      };

      newUserEntity.employer = newEmployerEntity;
      context.Employers.Add(newEmployerEntity);
      context.SaveChanges();

      employerId = newUserEntity.employer.employer_id;
    }

    return employerId;
  }
}