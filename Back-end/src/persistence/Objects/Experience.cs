namespace Back_end.Persistence.Objects;

public class Experience
{
  public string CompanyName { get; set; }
  public string PositionTitle { get; set; }
  public string? JobDescription { get; set; }

  public Experience(string companyName, string positionTitle, string? jobDescription)
  {
    this.CompanyName = companyName;
    this.PositionTitle = positionTitle;
    this. JobDescription = jobDescription;
  } 
}