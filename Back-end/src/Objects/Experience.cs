namespace Back_end.Objects;

public class Experience
{
    public int ExperienceId { get; set; }
    public string CompanyName { get; set; }
    public string PositionTitle { get; set; }
    public string JobDescription { get; set; }

    public Experience(int experienceId, string companyName, string positionTitle, string jobDescription)
    {
        this.ExperienceId = experienceId;
        this.CompanyName = companyName;
        this.PositionTitle = positionTitle;
        this.JobDescription = jobDescription;
    }
}