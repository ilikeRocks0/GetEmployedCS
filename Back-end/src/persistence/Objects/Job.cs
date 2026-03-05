namespace Back_end.Persistence.Objects;

public class Job
{
    public int JobId { get; }
    public string JobTitle { get; }
    public DateOnly? ApplicationDeadline { get; set; }
    public string? PosterName { get; set; }
    public string ApplicationLink { get; }
    public bool? HasRemote { get; set; }
    public bool? HasHybrid { get; set; }
    public string PositionType { get; }
    public string EmploymentType { get; }
    public List<string>? Locations { get; set; }
    public List<string>? ProgrammingLanguages { get; set; }
    public string JobDescription { get; }

    public Job(string jobTitle, DateOnly? applicationDeadline, string? posterName, string applicationLink, bool? hasRemote, bool? hasHybrid, string positionType, string employmentType, List<string>? locations, List<string>? programmingLanguages, string jobDescription)
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
        this.ProgrammingLanguages = programmingLanguages;
        this.JobDescription = jobDescription;
    }

    //Construct all non-nullable types.
    public Job(string jobTitle, string applicationLink, string positionType, string employmentType, string jobDescription)
    {
        this.JobTitle = jobTitle;
        this.ApplicationLink = applicationLink;
        this.PositionType = positionType;
        this.EmploymentType = employmentType;
        this.JobDescription = jobDescription;
    }
}