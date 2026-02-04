using Back_end.Services;

namespace Back_end.Persistance;

public class JobServiceStub : IJobService
{
    private readonly List<Job> JobListings =
    [
        new Job(1, "Software Engineer", "Tech Corp", "New York, NY"),
        new Job(2, "Data Scientist", "Data Inc", "San Francisco, CA"),
        new Job(3, "Product Manager", "Business Solutions", "Chicago, IL"),
        new Job(4, "UX Designer", "Creative Studio", "Austin, TX"),
        new Job(5, "DevOps Engineer", "Cloud Services", "Seattle, WA")
    ];

    public IReadOnlyList<Job> GetJobs(IReadOnlyDictionary<string, string>? filters = null) 
    {
        return JobListings;
    }

    public IReadOnlyList<Job> GetSavedJobs(IReadOnlyDictionary<string, string>? filters = null) 
    {
        return JobListings;
    }
}