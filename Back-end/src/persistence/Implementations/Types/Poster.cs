using Back_end.Persistence.Model;

namespace Back_end.Persistence.Implementations.Types;

public class Poster
{
    public Name? FullName { get; }
    public string? EmployerName { get; }
    public bool IsEmployer;

    public Poster(UserEntity? poster, bool employerPoster)
    {
        this.IsEmployer = employerPoster;

        if (poster != null && employerPoster && poster.employer != null)
        {
            this.EmployerName = poster.employer.employer_name;
        }
        else if (poster != null && !employerPoster && poster.jobSeeker != null)
        {
            this.FullName = new Name(poster.jobSeeker);
        }
    }

    public Poster(string name, bool employerPoster)
    {
        this.IsEmployer = employerPoster;

        if(employerPoster)
        {
            this.EmployerName = name;
        }
        else
        {
            this.FullName = new Name(name);
        }
    }

    public override string? ToString()
    {
        return this.IsEmployer ? this.EmployerName : this.FullName?.ToString();
    }
}