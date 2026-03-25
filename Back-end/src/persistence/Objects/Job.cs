namespace Back_end.Persistence.Objects;

public class Job
{
    public int? JobId { get; set; }
    public string JobTitle { get; set; }
    public DateOnly? ApplicationDeadline { get; set; }
    public string? PosterName { get; set; }
    public bool EmployerPoster { get; set; }
    public string ApplicationLink { get; set; }
    public bool? HasRemote { get; set; }
    public bool? HasHybrid { get; set; }
    public string PositionType { get; set; }
    public string EmploymentType { get; set; }
    public List<string>? Locations { get; set; }
    public List<string>? ProgrammingLanguages { get; set; }
    public string JobDescription { get; set; }

    public Job(string jobTitle, DateOnly? applicationDeadline, string? posterName, bool employerPoster, string applicationLink, bool? hasRemote, bool? hasHybrid, string positionType, string employmentType, List<string>? locations, List<string>? programmingLanguages, string jobDescription)
    {
        this.JobTitle = jobTitle;
        this.ApplicationDeadline = applicationDeadline;
        this.PosterName = posterName;
        this.EmployerPoster = employerPoster;
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