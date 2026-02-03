namespace Back_end.Persistence.Objects;

public class Job
{
  public string JobTitle { get; }
  public DateOnly? ApplicationDeadline { get; }
  public string? PosterName { get; }
  public string ApplicationLink { get; }
  public bool? HasRemote { get; }
  public bool? HasHybrid { get; }
  public string PositionType { get; }
  public string EmploymentType { get; }
  public List<string> Locations { get; }
  public string ProgrammingLanguage { get; }
  public string JobDescription { get; }

  public Job(string jobTitle, DateOnly? applicationDeadline, string? posterName, string applicationLink, bool? hasRemote, bool? hasHybrid, string positionType, string employmentType, List<string> locations, string programmingLanguage, string jobDescription)
  {
    this.JobTitle = jobTitle;
    this.ApplicationDeadline = applicationDeadline;
    this.PosterName = posterName;
    this.ApplicationLink = applicationLink;
    this.HasRemote = hasRemote;
    this.HasHybrid = hasHybrid;
    this.PositionType = positionType;
    this.EmploymentType = employmentType;
    this.Locations = locations;
    this.ProgrammingLanguage = programmingLanguage;
    this.JobDescription = jobDescription;
  }
}