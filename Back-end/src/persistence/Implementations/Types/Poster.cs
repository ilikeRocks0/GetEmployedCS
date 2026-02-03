namespace Back_end.Persistence.Implementations.Types;

public class Poster
{
  public string? Name { get; } = null;

  public Poster(UserEntity? poster, bool employerPoster)
  {
    if(poster != null && employerPoster && poster.employer != null)
    {
      Name = poster.employer.employer_name;
    }
    else if(poster != null && !employerPoster && poster.jobSeeker != null)
    {
      Name = poster.jobSeeker.first_name + " " + poster.jobSeeker.last_name;
    }
  }
}