using Back_end.Endpoints.Models;
using Back_end.Objects;

namespace Back_end.Services.Interfaces;

public interface IJobService
{
    /// Get a list of jobs. 
    /// <param name="filters">A dictionary of filter keys and values to apply when retrieving jobs. Supported keys include:
    /// - "SearchTerm": A keyword string to filter jobs by title.
    /// - "Languages": A comma-separated list of required programming languages or technologies.
    /// - "PositionTypes": A comma-separated list of roles to filter by (e.g., "front-end", "back-end").
    /// - "EmploymentTypes": A comma-separated list of types of contract to filter by (e.g., "full-time", "part-time").
    /// If null or empty, no filters will be applied and all jobs will be returned.
    IReadOnlyList<Job> GetJobs(IReadOnlyDictionary<string, string>? filters = null);
    
    /// Get a list of all saved jobs for a user within a list of jobs.
    /// <param name="filters">A dictionary of filter keys and values to apply when retrieving jobs. Supported keys include:
    /// - "SearchTerm": A keyword string to filter jobs by title.
    /// - "Languages": A comma-separated list of required programming languages or technologies.
    /// - "PositionTypes": A comma-separated list of roles to filter by (e.g., "front-end", "back-end").
    /// - "EmploymentTypes": A comma-separated list of types of contract to filter by (e.g., "full-time", "part-time").
    /// If null or empty, no filters will be applied and all jobs will be returned.
    IReadOnlyList<Job> GetJobsSavedSublist(IReadOnlyDictionary<string, string>? filters = null);

    /// Get a list of saved jobs for a user.
    /// <param name="filters">A dictionary of filter keys and values to apply when retrieving saved jobs. Supported keys include:
    /// - "UserId": The unique identifier of the user (required).
    /// - "SearchTerm": A keyword string to filter jobs by title.
    /// - "Languages": A comma-separated list of required programming languages or technologies.
    /// - "PositionTypes": A comma-separated list of roles to filter by (e.g., "front-end", "back-end").
    /// - "EmploymentTypes": A comma-separated list of types of contract to filter by (e.g., "full-time", "part-time").
    /// If null or missing "UserId", no saved jobs will be returned. Other filters are optional and will be applied if provided.
    IReadOnlyList<Job> GetSavedJobs(IReadOnlyDictionary<string, string>? filters = null);

    /// Get a count of jobs.
    /// <param name="filters">A dictionary of filter keys and values to apply when counting jobs. Supported keys include:
    /// - "SearchTerm": A keyword string to filter jobs by title.
    /// - "Languages": A comma-separated list of required programming languages or technologies.
    /// - "PositionTypes": A comma-separated list of roles to filter by (e.g., "front-end", "back-end").
    /// - "EmploymentTypes": A comma-separated list of types of contract to filter by (e.g., "full-time", "part-time").
    /// If null or empty, no filters will be applied and the total number of jobs will be returned.
    int GetNumberOfJobs(IReadOnlyDictionary<string, string>? filters = null);

    /// Get a count of saved jobs.
    /// <param name="filters">A dictionary of filter keys and values to apply when counting saved jobs. Supported keys include:
    /// - "UserId": The unique identifier of the user (required).
    /// - "SearchTerm": A keyword string to filter jobs by title.
    /// - "Languages": A comma-separated list of required programming languages or technologies.
    /// - "PositionTypes": A comma-separated list of roles to filter by (e.g., "front-end", "back-end").
    /// - "EmploymentTypes": A comma-separated list of types of contract to filter by (e.g., "full-time", "part-time").
    /// If null or missing "UserId", the count will be zero. Other filters are optional and will be applied if provided.
    int GetNumberOfSavedJobs(IReadOnlyDictionary<string, string>? filters = null);
    
    /// Get a list of all programming languages used in jobs.
    List<string> GetProgrammingLanguages();

    /// Delete a job a user has added.
    /// <param name="userId">An id of a user to delete the job from.
    /// <param name="filters">An id of a job to delete
    /// If the job was not added by the user, the deletion will fail.
    void DeleteJob(int userId, int jobId);
}

