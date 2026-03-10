using Back_end.Endpoints.Models;
using Back_end.Services.Interfaces;

namespace Back_end.Endpoints;

public static class JobEndpoints
{
    public static void MapJobEndpoints(this IEndpointRouteBuilder routes)
    {
        // This endpoint retrieves a list of jobs based on the provided filters. 
        // The filters are passed as query parameters and can include:
        // - "SearchTerm": A keyword string to filter jobs by title.
        // - "Languages": A comma-separated list of required programming languages or technologies.
        // - "PositionTypes": A comma-separated list of roles to filter by (e.g., "front-end", "back-end").
        // - "EmploymentTypes": A comma-separated list of types of contract to filter by (e.g., "full-time", "part-time").
        // If no filters are provided, all jobs will be returned.
        routes.MapGet("/api/jobs", (HttpContext context, IJobService jobService) =>
        {
            var filters = context.Request.Query.ToDictionary(query => query.Key, query => query.Value.ToString());
            var results = jobService.GetJobs(filters.Count > 0 ? filters : null);
            return new JobsResponse(results.jobList, results.savedJobIds);
        })
            .WithName("GetJobListings")
            .WithTags("Jobs")
            .WithOpenApi();

        // This endpoint retrieves the saved jobs for a user based on the provided filters. 
        // The filters are the same as those used in GetJobs, but "SeekerId" is required to identify the user whose saved jobs are being requested.
        // If "SeekerId" is missing or invalid, no saved jobs will be returned.
        routes.MapGet("/api/jobs/saved", (HttpContext context, IJobService jobService) =>
        {
            var filters = context.Request.Query.ToDictionary(query => query.Key, query => query.Value.ToString());
            return jobService.GetSavedJobs(filters.Count > 0 ? filters : null);
        })
            .WithName("GetSavedJobs")
            .WithTags("Jobs")
            .WithOpenApi();
        
        
        routes.MapGet("/api/jobs/number", (HttpContext context, IJobService jobService) =>
        {
            var filters = context.Request.Query.ToDictionary(query => query.Key, query => query.Value.ToString());
            return jobService.GetNumberOfJobs(filters.Count > 0 ? filters : null);
        })
            .WithName("GetNumberOfJobs")
            .WithTags("Jobs")
            .WithOpenApi();

        routes.MapGet("/api/jobs/saved/number", (HttpContext context, IJobService jobService) =>
        {
            var filters = context.Request.Query.ToDictionary(query => query.Key, query => query.Value.ToString());
            return jobService.GetNumberOfSavedJobs(filters.Count > 0 ? filters : null);
        })
            .WithName("GetNumberOfSavedJobs")
            .WithTags("Jobs")
            .WithOpenApi();

        routes.MapGet("/api/jobs/languages", (IJobService jobService) =>
        {
            return jobService.GetProgrammingLanguages();
        })
            .WithName("GetProgrammingLanguages")
            .WithTags("Jobs")
            .WithOpenApi();
    }

    public static void MapJobGameEndpoints(this IEndpointRouteBuilder routes)
    {
        // This endpoint initializes the job list for the game based on the provided filters and returns a random job to start the game. 
        // The filters are the same as those used in GetJobs.
        routes.MapPost("/api/job/game", (CurrentUser currentUser, HttpContext context, IJobGameConnector jobGameConnector) =>
        {
            var filters = context.Request.Query.ToDictionary(query => query.Key, query => query.Value.ToString());
            return jobGameConnector.InitializeJobGame(currentUser, filters.Count > 0 ? filters : null);
        })
            .WithName("InitializeJobGame")
            .WithTags("Job Game")
            .WithOpenApi();

        // this endpoint allows the user to reject the current job in the game and receive the next job.
        routes.MapPost("/api/job/game/reject", (GameJob gameJob, IJobGameConnector jobGameConnector) =>
        {
            return jobGameConnector.RejectJob(gameJob);
        })
            .WithName("RejectJob")
            .WithTags("Job Game")
            .WithOpenApi();
        
        // this endpoint allows the user to accept the current job in the game and receive the next job.
        routes.MapPost("/api/job/game/accept", (GameJob gameJob, IJobGameConnector jobGameConnector) =>
        {            
            return jobGameConnector.AcceptJob(gameJob);
        })
            .WithName("AcceptJob")
            .WithTags("Job Game")
            .WithOpenApi();

        // this endpoint allows the user to retrieve the current game statistics, including the number of accepted and rejected jobs.
        routes.MapPost("/api/job/game/stats", (CurrentUser currentUser, IJobGameConnector jobGameConnector) =>
        {            
            return jobGameConnector.GetGameStats(currentUser);
        })
            .WithName("GetGameStats")
            .WithTags("Job Game")
            .WithOpenApi();
    }
}