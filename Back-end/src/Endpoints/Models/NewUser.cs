namespace Back_end.Endpoints.Models.NewUser;
public class NewUser
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; } 
    public string LastName { get; set; }
    public string EmployerName { get; set; }
    public bool IsEmployer { get; set; }
    public string Email { get; set; }

    public NewUser(string username, string password, string email, bool IsEmployer, string firstName, string lastName, string employerName)
  {
    this.Username = username;
    this.Password = password;
    this.Email = email;
    this.IsEmployer = IsEmployer;
    this.FirstName = firstName;
    this.LastName = lastName;
    this.EmployerName = employerName;
    
  }
}