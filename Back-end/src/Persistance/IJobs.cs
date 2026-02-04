namespace Back_end.Persistance;

public class Job
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Company { get; set; }
    public string Location { get; set; }

    public Job(int id, string title, string company, string location)
    {
        Id = id;
        Title = title;
        Company = company;
        Location = location;
    }
    
}