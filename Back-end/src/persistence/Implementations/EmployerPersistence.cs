using System.Data.Entity;
using Back_end.Persistence.Interface;
using Back_end.Persistence.Model;
using Back_end.Persistence.Objects;

namespace Back_end.Persistence.Implementations;

public class EmployerPersistence : IEmployerPersistence
{
  private IConfiguration config;

  public EmployerPersistence(IConfiguration config)
  {
    this.config = config;
  }

  private Employer EmployerEntityToObject(EmployerEntity e)
  {
    string? about = null;

    using (AppDbContext context = new(this.config))
    {
      about = context.Users
        .Where(n => n.user_id == e.user_id)
        .Include(n => n.about_string)
        .Single()
        .about_string;
    }

    return new Employer(e.employer_name, about);
  }

  public Employer GetEmployer(int employerId)
  {
    Employer? employer = null;

    using (AppDbContext context = new(this.config))
    {
      EmployerEntity employerEntity = context.Employers
        .Where(e => e.employer_id == employerId)
        .Single();

      employer = EmployerEntityToObject(employerEntity);
    }

    return employer;
  }

  public void CreateEmployer(int userId, Employer employer)
  {
    using (AppDbContext context = new(this.config))
    {
      EmployerEntity employerEntity = new()
      {
        user_id = userId,
        employer_name = employer.Name
      };

      context.Employers.Add(employerEntity);
      
      context.SaveChanges();
    }
  }
}