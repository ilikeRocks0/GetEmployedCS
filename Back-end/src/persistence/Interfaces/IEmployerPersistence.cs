using Back_end.Persistence.Objects;

namespace Back_end.Persistence.Interface;

public interface IEmployerPersistence
{
  public Employer GetEmployer(int employerId);
  public void CreateEmployer(int userId, Employer employer);
}