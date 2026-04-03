using Back_end.Objects;

namespace Back_end.Endpoints.Models;

public class Profile
{
    public int UserId { get; set; }
    public string Username { get; set; }
    public string Email { get; set; } 
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Bio { get; set; }
    public List<Experience>? Experiences { get; set; }
    public bool IsEmployer { get; set; }
    public string? EmployerName { get; set; }
    public bool IsSelf { get; set; }
    public List<Job>? PostedJobs { get; set; }

    public Profile(int userId, string username, string email, string? firstName, string? lastName, string? bio, List<Experience>? experiences, bool isEmployer, string? EmployerName, bool isSelf = false, List<Job>? postedJobs = null)
    {
        this.UserId = userId;
        this.Username = username;
        this.Email = email;
        this.FirstName = firstName;
        this.LastName = lastName;
        this.Bio = bio;
        this.Experiences = experiences;
        this.IsEmployer = isEmployer;
        this.EmployerName = EmployerName;
        this.IsSelf = isSelf;
        this.PostedJobs = postedJobs;
    }
}