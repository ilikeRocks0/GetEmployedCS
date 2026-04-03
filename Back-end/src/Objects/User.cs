namespace Back_end.Objects;

public class User
{
    public int UserId { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string? FirstName { get; set; } = null;
    public string? LastName { get; set; } = null;
    public List<Experience>? Experiences { get; set; } = null;
    public string? EmployerName { get; set; } = null;
    public bool IsEmployer { get; set; }
    public string About { get; set; }
    public string Email { get; set; }

    protected User(int userId, string email, string username, string password, string about)
    {
        this.UserId = userId;
        this.Email = email;
        this.Username = username;
        this.Password = password;
        this.About = about;
    }

    public User(int userId, string email, string username, string password, string about, string firstName, string lastName, List<Experience> experiences) : this(userId, email, username, password, about)
    {
        this.FirstName = firstName;
        this.LastName = lastName;
        this.Experiences = experiences;
        this.IsEmployer = false;
    }

    public User(int userId, string email, string username, string password, string about, string employerName) : this(userId, email, username, password, about)
    {
        this.EmployerName = employerName;
        this.IsEmployer = true;
    }
}