using Back_end.Persistence.Objects;

namespace Back_end.Persistence.Interfaces;

public interface IJobSeekerPersistence
{
  public JobSeeker GetJobSeeker(int seekerId);
  public void CreateJobSeeker(int userId, JobSeeker jobSeeker);
}