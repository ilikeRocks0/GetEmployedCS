using System.Data.Entity;
using Back_end.Persistence.Implementations;
using Back_end.Persistence.Interfaces;
using Back_end.Persistence.Model;
using Back_end.Persistence.Objects;

public class JobSeekerPersistence : IJobSeekerPersistence
{
  private IConfiguration config;

  public JobSeekerPersistence(IConfiguration config)
  {
    this.config = config;
  }

  private JobSeeker jobSeekerEntityToObject(JobSeekerEntity e)
  {
    List<Experience> experiences = new List<Experience>();
    List<ExperienceEntity> experienceEntities = new List<ExperienceEntity>();
    string? about = null;

    // Query to get the list of experiences and about string
    using (AppDbContext context = new(this.config))
    {
      experienceEntities = context.Experiences.Where(n => n.seeker_id == e.seeker_id).ToList();
      about = context.Users
        .Where(n => n.user_id == e.user_id)
        .Include(n => n.about_string)
        .Single()
        .about_string;
    }

    // Convert experience entities to business objects
    experienceEntities.ForEach(n => experiences.Add(new Experience(n.company_name, n.position_title, n.job_description)));

    return new JobSeeker(
      e.first_name,
      e.last_name,
      about,
      experiences
    );
  }

  public JobSeeker GetJobSeeker(int seekerId)
  {
    JobSeeker? seeker = null;

    using (AppDbContext context = new(this.config))
    {
      JobSeekerEntity jobSeekerEntity = context.JobSeekers.Where(e => e.seeker_id == seekerId).Single();
      
      seeker = jobSeekerEntityToObject(jobSeekerEntity);
    }

    return seeker;
  }

  public void CreateJobSeeker(int userId, JobSeeker jobSeeker)
  {
    using (AppDbContext context = new(this.config))
    {
      JobSeekerEntity jobSeekerEntity = new()
      {
        user_id = userId,
        first_name = jobSeeker.FirstName,
        last_name = jobSeeker.LastName,
        likes = new List<LikeEntity>()
      };

      context.JobSeekers.Add(jobSeekerEntity);

      context.SaveChanges();
    }
  }
}