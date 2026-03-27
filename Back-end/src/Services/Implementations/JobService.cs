using Back_end.Persistence.Interfaces;
using Back_end.Persistence.Objects;
using Back_end.Services.Interfaces;
using Back_end.Util;

public class JobService(IJobPersistence jobPersistence) : IJobService
{
    public IReadOnlyList<Job> GetJobs(IReadOnlyDictionary<string, string>? filters = null)
    {
        var (_, searchTerm, languages, positions, employments, startIndex) = ParseFilters(filters);
    
        return jobPersistence.GetJobs(searchTerm, languages, positions, employments, startIndex, AppConfig.ITEMS_PER_PAGE);
    }

    public IReadOnlyList<Job> GetJobsSavedSublist(IReadOnlyDictionary<string, string>? filters = null)
    {
        var (UserId, searchTerm, languages, positions, employments, startIndex) = ParseFilters(filters);
        var jobs = jobPersistence.GetJobs(searchTerm, languages, positions, employments, startIndex, AppConfig.ITEMS_PER_PAGE);
        return jobPersistence.GetJobsSavedSublist(jobs, UserId);
    }

    public IReadOnlyList<Job> GetSavedJobs(IReadOnlyDictionary<string, string>? filters = null)
    {
        var (UserId, searchTerm, languages, positions, employments, startIndex) = ParseFilters(filters);
        
        return jobPersistence.GetSavedJobs(UserId, searchTerm, languages, positions, employments, startIndex, AppConfig.ITEMS_PER_PAGE);
    }

    public int GetNumberOfJobs(IReadOnlyDictionary<string, string>? filters = null)
    {
        var (_, searchTerm, languages, positions, employments, _) = ParseFilters(filters);
    
        return jobPersistence.GetNumberOfJobs(searchTerm, languages, positions, employments);
    }

    public int GetNumberOfSavedJobs(IReadOnlyDictionary<string, string>? filters = null)
    {
        var (UserId, searchTerm, languages, positions, employments, _) = ParseFilters(filters);
        
        return jobPersistence.GetNumberOfSavedJobs(UserId, searchTerm, languages, positions, employments);
    }

    //<param name="UserId">The unique identifier of the user.</param>
    //<param name="searchTerm">A keyword string to filter jobs by title.</param>
    //<param name="languages">A list of required programming languages or technologies.</param>
    //<param name="positionTypes">The roles to filter by (e.g., "front-end", "back-end").</param>
    //<param name="employmentTypes">The types of contract to filter by (e.g., "full-time", "part-time).</param>
    private static (int userId, string searchTerm, List<string> languages, List<string> positions, List<string> employments, int startIndex) ParseFilters(IReadOnlyDictionary<string, string>? filters)
    {
        if (filters == null)
        {
            return (0, string.Empty, [], [], [], 0);
        }

        return (
            int.TryParse(filters.GetValueOrDefault(AppConfig.FilterKeys.USERID), out var id) ? id : 0,
            filters.GetValueOrDefault(AppConfig.FilterKeys.SEARCH_TERM, string.Empty),
            filters.GetValueOrDefault(AppConfig.FilterKeys.LANGUAGES)?.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList() ?? [],
            filters.GetValueOrDefault(AppConfig.FilterKeys.POSITION_TYPES)?.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList() ?? [],
            filters.GetValueOrDefault(AppConfig.FilterKeys.EMPLOYMENT_TYPES)?.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList() ?? [],
            int.TryParse(filters.GetValueOrDefault(AppConfig.FilterKeys.PAGE_NUMBER), out var pageNumber) ? ((pageNumber-1) * AppConfig.ITEMS_PER_PAGE) : 0
        );
    }

    public List<string> GetProgrammingLanguages()
    {
        return jobPersistence.GetProgrammingLanguages();
    }

    public void DeleteJob(int userId, int jobId)
    {
        if (!jobPersistence.IsJobOwner(userId, jobId))
            throw new UnauthorizedAccessException("Job does not belong to this user.");

        jobPersistence.DeleteJob(jobId);
    }
}