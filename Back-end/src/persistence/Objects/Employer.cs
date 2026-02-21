namespace Back_end.Persistence.Objects;

public class Employer
{
  public string Name { get; }
  public string? About { get; }

  public Employer(string name, string? about)
  {
    this.Name = name;
    this.About = about;
  }
}