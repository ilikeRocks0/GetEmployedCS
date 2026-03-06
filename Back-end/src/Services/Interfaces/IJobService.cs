using Back_end.Persistence.Objects;

namespace Back_end.Services.Interfaces;

public interface IJobService
{
    /// <param name="filters">A dictionary of filter keys and values to apply when retrieving jobs. Supported keys include:
    /// - "SearchTerm": A keyword string to filter jobs by title.
    /// - "Languages": A comma-separated list of required programming languages or technologies.
    /// - "PositionTypes": A comma-separated list of roles to filter by (e.g., "front-end", "back-end").
    /// - "EmploymentTypes": A comma-separated list of types of contract to filter by (e.g., "full-time", "part-time").
    /// If null or empty, no filters will be applied and all jobs will be returned.
    IReadOnlyList<Job> GetJobs(IReadOnlyDictionary<string, string>? filters = null);
    /// <param name="filters">A dictionary of filter keys and values to apply when retrieving saved jobs. Supported keys include:
    /// - "UserId": The unique identifier of the user (required).
    /// - "SearchTerm": A keyword string to filter jobs by title.
    /// - "Languages": A comma-separated list of required programming languages or technologies.
    /// - "PositionTypes": A comma-separated list of roles to filter by (e.g., "front-end", "back-end").
    /// - "EmploymentTypes": A comma-separated list of types of contract to filter by (e.g., "full-time", "part-time").
    /// If null or missing "UserId", no saved jobs will be returned. Other filters are optional and will be applied if provided.
    IReadOnlyList<Job> GetSavedJobs(IReadOnlyDictionary<string, string>? filters = null);

    /// <param name="filters">A dictionary of filter keys and values to apply when counting jobs. Supported keys include:
    /// - "SearchTerm": A keyword string to filter jobs by title.
    /// - "Languages": A comma-separated list of required programming languages or technologies.
    /// - "PositionTypes": A comma-separated list of roles to filter by (e.g., "front-end", "back-end").
    /// - "EmploymentTypes": A comma-separated list of types of contract to filter by (e.g., "full-time", "part-time").
    /// If null or empty, no filters will be applied and the total number of jobs will be returned.
    int GetNumberOfJobs(IReadOnlyDictionary<string, string>? filters = null);

    /// <param name="filters">A dictionary of filter keys and values to apply when counting saved jobs. Supported keys include:
    /// - "UserId": The unique identifier of the user (required).
    /// - "SearchTerm": A keyword string to filter jobs by title.
    /// - "Languages": A comma-separated list of required programming languages or technologies.
    /// - "PositionTypes": A comma-separated list of roles to filter by (e.g., "front-end", "back-end").
    /// - "EmploymentTypes": A comma-separated list of types of contract to filter by (e.g., "full-time", "part-time").
    /// If null or missing "UserId", the count will be zero. Other filters are optional and will be applied if provided.
    int GetNumberOfSavedJobs(IReadOnlyDictionary<string, string>? filters = null);
    
    
    List<string> GetProgrammingLanguages();
}

