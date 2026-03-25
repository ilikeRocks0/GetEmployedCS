namespace Back_end.Endpoints.Models;
public class NewJob
{
    public string Title { get; set; }
    public string Deadline { get; set; }
    public string ApplicationLink { get; set; } 
    public bool HasRemote { get; set; }
    public bool HasHybrid { get; set; }
    public string PositionType { get; set; }
    public string EmploymentType { get; set; }
    public List<string> Locations { get; set; }
    public List<string> ProgrammingLanguages { get; set; }
    public string JobDescription { get; set; }
    public bool EmployerPoster { get; set; }

    public NewJob(string title, string deadline, string applicationLink, bool hasRemote, bool hasHybrid, string positionType, string employmentType, List<string> locations, List<string> programmingLanguages, string jobDescription, bool employerPoster)
    {
        this.Title = title;
        this.Deadline = deadline;
        this.ApplicationLink = applicationLink;
        this.HasRemote = hasRemote;
        this.HasHybrid = hasHybrid;
        this.PositionType = positionType;
        this.EmploymentType = employmentType;
        this.Locations = locations;
        this.ProgrammingLanguages = programmingLanguages;
        this.JobDescription = jobDescription;
        this.EmployerPoster = employerPoster;   
    }
}