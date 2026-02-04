using Back_end.Persistance;

namespace Back_end.Services;

public class JobService
{
    private static readonly List<Job> JobListings =
    [
        new Job(1, "Software Engineer", "Tech Corp", "New York, NY"),
        new Job(2, "Data Scientist", "Data Inc", "San Francisco, CA"),
        new Job(3, "Product Manager", "Business Solutions", "Chicago, IL"),
        new Job(4, "UX Designer", "Creative Studio", "Austin, TX"),
        new Job(5, "DevOps Engineer", "Cloud Services", "Seattle, WA")
    ];

    /// <param name="filters">Query params as dictionary. Pass to database quary when ready.</param>
    public IReadOnlyList<Job> GetJobs(IReadOnlyDictionary<string, string>? filters = null)
    {
        return JobListings; // TODO: ORM will use filters
    }
    /// <param name="filters">Query params as dictionary. Pass to database quary when ready.</param>
    public IReadOnlyList<Job> GetSavedJobs(IReadOnlyDictionary<string, string>? filters = null)
    {
        return JobListings; // TODO: database quary will use filters
    }
}
