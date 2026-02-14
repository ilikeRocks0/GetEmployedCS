using Back_end.Persistence.Interfaces;
using Back_end.Persistence.Objects;
using Back_end.Services.Interfaces;

public class JobService(IJobPersistence jobPersistence) : IJobService
{
    public IReadOnlyList<Job> GetJobs(IReadOnlyDictionary<string, string>? filters = null)
    {
        var (_, searchTerm, languages, positions, employments) = ParseFilters(filters);
    
        return jobPersistence.GetJobs(searchTerm, languages, positions, employments);
    }

    public IReadOnlyList<Job> GetSavedJobs(IReadOnlyDictionary<string, string>? filters = null)
    {
        var (seekerId, searchTerm, languages, positions, employments) = ParseFilters(filters);
        
        return jobPersistence.GetSavedJobs(seekerId, searchTerm, languages, positions, employments);
    }

    //<param name="seekerId">The unique identifier of the user.</param>
    //<param name="searchTerm">A keyword string to filter jobs by title.</param>
    //<param name="languages">A list of required programming languages or technologies.</param>
    //<param name="positionTypes">The roles to filter by (e.g., "front-end", "back-end").</param>
    //<param name="employmentTypes">The types of contract to filter by (e.g., "full-time", "part-time).</param>
    private static (int SeekerId, string SearchTerm, List<string> Languages, List<string> Positions, List<string> Employments) ParseFilters(IReadOnlyDictionary<string, string>? filters)
    {
        if (filters == null) return (0, string.Empty, [], [], []);

        return (
            int.TryParse(filters.GetValueOrDefault(AppConfig.FilterKeys.SeekerId), out var id) ? id : 0,
            filters.GetValueOrDefault(AppConfig.FilterKeys.SearchTerm, string.Empty),
            filters.GetValueOrDefault(AppConfig.FilterKeys.Languages)?.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList() ?? [],
            filters.GetValueOrDefault(AppConfig.FilterKeys.PositionTypes)?.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList() ?? [],
            filters.GetValueOrDefault(AppConfig.FilterKeys.EmploymentTypes)?.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList() ?? []
        );
    }
}