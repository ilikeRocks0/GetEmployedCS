namespace Back_end.Endpoints.Models;

public class UpdateUserRequest
{
    public string? Username { get; set; }
    public string? About { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? EmployerName { get; set; }
}
