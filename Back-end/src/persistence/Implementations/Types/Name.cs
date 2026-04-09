using Back_end.Persistence.Model;

namespace Back_end.Persistence.Implementations.Types;

public class Name
{
    public string FirstName { get; }
    public string LastName { get; }

    public Name(JobSeekerEntity jobSeeker)
    {
        this.FirstName = jobSeeker.first_name;
        this.LastName = jobSeeker.last_name;
    }

    public Name(string fullName)
    {
        string[] splitName = fullName.Split(' ', StringSplitOptions.TrimEntries);
        
        // Concatenates all names before the last element as the first name if there are more than 2 elements in splitName
        this.FirstName = string.Join(" ", splitName[..^1]);

        
        if(splitName.Length > 1)
        {
            this.LastName = splitName[splitName.Length - 1];
        }
        else
        {
            this.LastName = "";
        }
    }

    public override string ToString()
    {
        return this.FirstName + " " + this.LastName;
    }
}