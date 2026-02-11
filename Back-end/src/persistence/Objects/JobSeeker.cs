namespace Back_end.Persistence.Objects;

public class JobSeeker
{
  public string FirstName { get; set; }
  public string LastName { get; set; }
  public string? About { get; set; }
  public List<Experience> Experiences { get; set; }

  public JobSeeker(string firstName, string lastName, string? about, List<Experience> experiences)
  {
    this.FirstName = firstName;
    this.LastName = lastName;
    this.About = about;
    this.Experiences = experiences;
  }
}