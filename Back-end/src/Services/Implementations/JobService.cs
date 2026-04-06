using Back_end.Persistence.Interfaces;
using Back_end.Objects;
using Back_end.Services.Interfaces;
using Back_end.Util;

namespace Back_end.Services.Implementations;

public class JobService(IJobPersistence jobPersistence) : IJobService
{
    /// Get a list of jobs. 
    /// <param name="filters">A dictionary of filter keys and values to apply when retrieving jobs. Supported keys include:
    /// - "SearchTerm": A keyword string to filter jobs by title.
    /// - "Languages": A comma-separated list of required programming languages or technologies.
    /// - "PositionTypes": A comma-separated list of roles to filter by (e.g., "front-end", "back-end").
    /// - "EmploymentTypes": A comma-separated list of types of contract to filter by (e.g., "full-time", "part-time").
    /// If null or empty, no filters will be applied and all jobs will be returned.
    public IReadOnlyList<Job> GetJobs(IReadOnlyDictionary<string, string>? filters = null)
    {
        var (_, searchTerm, languages, positions, employments, startIndex) = ParseFilters(filters);
    
        return jobPersistence.GetJobs(searchTerm, languages, positions, employments, startIndex, AppConfig.ITEMS_PER_PAGE);
    }

    /// Get a list of all saved jobs for a user within a list of jobs.
    /// <param name="filters">A dictionary of filter keys and values to apply when retrieving jobs. Supported keys include:
    /// - "SearchTerm": A keyword string to filter jobs by title.
    /// - "Languages": A comma-separated list of required programming languages or technologies.
    /// - "PositionTypes": A comma-separated list of roles to filter by (e.g., "front-end", "back-end").
    /// - "EmploymentTypes": A comma-separated list of types of contract to filter by (e.g., "full-time", "part-time").
    /// If null or empty, no filters will be applied and all jobs will be returned.
    public IReadOnlyList<Job> GetJobsSavedSublist(IReadOnlyDictionary<string, string>? filters = null)
    {
        var (UserId, searchTerm, languages, positions, employments, startIndex) = ParseFilters(filters);
        var jobs = jobPersistence.GetJobs(searchTerm, languages, positions, employments, startIndex, AppConfig.ITEMS_PER_PAGE);
        return jobPersistence.GetJobsSavedSublist(jobs, UserId);
    }

    /// Get a list of saved jobs for a user.
    /// <param name="filters">A dictionary of filter keys and values to apply when retrieving saved jobs. Supported keys include:
    /// - "UserId": The unique identifier of the user (required).
    /// - "SearchTerm": A keyword string to filter jobs by title.
    /// - "Languages": A comma-separated list of required programming languages or technologies.
    /// - "PositionTypes": A comma-separated list of roles to filter by (e.g., "front-end", "back-end").
    /// - "EmploymentTypes": A comma-separated list of types of contract to filter by (e.g., "full-time", "part-time").
    /// If null or missing "UserId", no saved jobs will be returned. Other filters are optional and will be applied if provided.
    public IReadOnlyList<Job> GetSavedJobs(IReadOnlyDictionary<string, string>? filters = null)
    {
        var (UserId, searchTerm, languages, positions, employments, startIndex) = ParseFilters(filters);
        
        return jobPersistence.GetSavedJobs(UserId, searchTerm, languages, positions, employments, startIndex, AppConfig.ITEMS_PER_PAGE);
    }

    /// Get a count of jobs.
    /// <param name="filters">A dictionary of filter keys and values to apply when counting jobs. Supported keys include:
    /// - "SearchTerm": A keyword string to filter jobs by title.
    /// - "Languages": A comma-separated list of required programming languages or technologies.
    /// - "PositionTypes": A comma-separated list of roles to filter by (e.g., "front-end", "back-end").
    /// - "EmploymentTypes": A comma-separated list of types of contract to filter by (e.g., "full-time", "part-time").
    /// If null or empty, no filters will be applied and the total number of jobs will be returned.
    public int GetNumberOfJobs(IReadOnlyDictionary<string, string>? filters = null)
    {
        var (_, searchTerm, languages, positions, employments, _) = ParseFilters(filters);
    
        return jobPersistence.GetNumberOfJobs(searchTerm, languages, positions, employments);
    }

    /// Get a count of saved jobs.
    /// <param name="filters">A dictionary of filter keys and values to apply when counting saved jobs. Supported keys include:
    /// - "UserId": The unique identifier of the user (required).
    /// - "SearchTerm": A keyword string to filter jobs by title.
    /// - "Languages": A comma-separated list of required programming languages or technologies.
    /// - "PositionTypes": A comma-separated list of roles to filter by (e.g., "front-end", "back-end").
    /// - "EmploymentTypes": A comma-separated list of types of contract to filter by (e.g., "full-time", "part-time").
    /// If null or missing "UserId", the count will be zero. Other filters are optional and will be applied if provided.
    public int GetNumberOfSavedJobs(IReadOnlyDictionary<string, string>? filters = null)
    {
        var (UserId, searchTerm, languages, positions, employments, _) = ParseFilters(filters);
        
        return jobPersistence.GetNumberOfSavedJobs(UserId, searchTerm, languages, positions, employments);
    }

    /// Extract individual elements from filters.
    /// <param name="filters">A dictionary of strings to extract elements from. Extracts the following:    /// <param name="UserId">The unique identifier of the user.</param>
    /// - "userId": The unique identifier of the user.
    /// - "searchTerm": A keyword string to filter jobs by title.
    /// - "languages": A list of required programming languages or technologies.
    /// - "positions": The roles to filter by (e.g., "front-end", "back-end").
    /// - "employments">: The types of contract to filter by (e.g., "full-time", "part-time).
    /// - "startIndex": The index of the record in the Jobs table to start returning entities.
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

    /// Get a list of all programming languages used in jobs.
    public List<string> GetProgrammingLanguages()
    {
        return jobPersistence.GetProgrammingLanguages();
    }

    /// Delete a job a user has added.
    /// <param name="userId">An id of a user to delete the job from.
    /// <param name="filters">An id of a job to delete
    /// If the job was not added by the user, the deletion will fail.
    public void DeleteJob(int userId, int jobId)
    {
        if (!jobPersistence.IsJobOwner(userId, jobId))
            throw new UnauthorizedAccessException("Job does not belong to this user.");

        jobPersistence.DeleteJob(jobId);
    }
}