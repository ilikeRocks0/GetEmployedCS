using Back_end.Endpoints.Models;
using Back_end.Services.Interfaces;
using Back_end.Util;

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
            return jobService.GetJobs(filters.Count > 0 ? filters : null);
        })
            .WithName("GetJobListings")
            .WithTags("Jobs")
            .WithOpenApi()
            .RequireAuthorization();

        // This endpoint retrieves a sublist of a user's saved jobs within the list of jobs based on the provided filters. 
        // The filters are passed as query parameters and can include:
        // - "SearchTerm": A keyword string to filter jobs by title.
        // - "Languages": A comma-separated list of required programming languages or technologies.
        // - "PositionTypes": A comma-separated list of roles to filter by (e.g., "front-end", "back-end").
        // - "EmploymentTypes": A comma-separated list of types of contract to filter by (e.g., "full-time", "part-time").
        // If no filters are provided, all of the user's saved jobs within the jobs list will be returned.
        routes.MapGet("/api/jobs/saved/sublist", (HttpContext context, IJobService jobService) =>
        {
            var filters = context.Request.Query.ToDictionary(query => query.Key, query => query.Value.ToString());
            var userId = context.User.FindFirst("UserId")?.Value;
            filters[AppConfig.FilterKeys.USERID] = userId ?? "0";
            return jobService.GetJobsSavedSublist(filters.Count > 0 ? filters : null);
        })
            .WithName("GetSavedJobListings")
            .WithTags("Jobs")
            .WithOpenApi()
            .RequireAuthorization();

        // This endpoint retrieves the saved jobs for a user based on the provided filters. 
        // The filters are the same as those used in GetJobs, but "SeekerId" is required to identify the user whose saved jobs are being requested.
        // If "SeekerId" is missing or invalid, no saved jobs will be returned.
        routes.MapGet("/api/jobs/saved", (HttpContext context, IJobService jobService) =>
        {
            var filters = context.Request.Query.ToDictionary(query => query.Key, query => query.Value.ToString());
            var userId = context.User.FindFirst("UserId")?.Value;
            filters[AppConfig.FilterKeys.USERID] = userId ?? "0";
            return jobService.GetSavedJobs(filters.Count > 0 ? filters : null);
        })
            .WithName("GetSavedJobs")
            .WithTags("Jobs")
            .WithOpenApi()
            .RequireAuthorization();
        
        
        routes.MapGet("/api/jobs/number", (HttpContext context, IJobService jobService) =>
        {
            var filters = context.Request.Query.ToDictionary(query => query.Key, query => query.Value.ToString());
            return jobService.GetNumberOfJobs(filters.Count > 0 ? filters : null);
        })
            .WithName("GetNumberOfJobs")
            .WithTags("Jobs")
            .WithOpenApi()
            .RequireAuthorization();

        routes.MapGet("/api/jobs/saved/number", (HttpContext context, IJobService jobService) =>
        {
            var filters = context.Request.Query.ToDictionary(query => query.Key, query => query.Value.ToString());
            var userId = context.User.FindFirst("UserId")?.Value;
            filters[AppConfig.FilterKeys.USERID] = userId ?? "0";
            return jobService.GetNumberOfSavedJobs(filters.Count > 0 ? filters : null);
        })
            .WithName("GetNumberOfSavedJobs")
            .WithTags("Jobs")
            .WithOpenApi()
            .RequireAuthorization();

        routes.MapGet("/api/jobs/languages", (IJobService jobService) =>
        {
            return jobService.GetProgrammingLanguages();
        })
            .WithName("GetProgrammingLanguages")
            .WithTags("Jobs")
            .WithOpenApi()
            .RequireAuthorization();
    }

    public static void MapJobGameEndpoints(this IEndpointRouteBuilder routes)
    {
        // This endpoint initializes the job list for the game based on the provided filters and returns a random job to start the game. 
        // The filters are the same as those used in GetJobs.
        routes.MapPost("/api/job/game", (HttpContext context, IJobGameService jobGameService) =>
        {
            var filters = context.Request.Query.ToDictionary(query => query.Key, query => query.Value.ToString());
            var userIdStr = context.User.FindFirst("UserId")?.Value;
            if (!int.TryParse(userIdStr, out var userId))
                return Results.Unauthorized();
            filters[AppConfig.FilterKeys.USERID] = userIdStr;
            return Results.Ok(jobGameService.InitializeJobGame(new CurrentUser(userId), filters.Count > 0 ? filters : null));
        })
            .WithName("InitializeJobGame")
            .WithTags("Job Game")
            .WithOpenApi()
            .RequireAuthorization();

        // this endpoint allows the user to reject the current job in the game and receive the next job.
        routes.MapPost("/api/job/game/reject", (JobRequest jobRequest, HttpContext context, IJobGameService jobGameService) =>
        {
            var userIdStr = context.User.FindFirst("UserId")?.Value;
            if (!int.TryParse(userIdStr, out var userId))
                return Results.Unauthorized();
            return Results.Ok(jobGameService.RejectJob(new GameJob(userId, jobRequest.JobId)));
        })
            .WithName("RejectJob")
            .WithTags("Job Game")
            .WithOpenApi()
            .RequireAuthorization();

        // this endpoint allows the user to accept the current job in the game and receive the next job.
        routes.MapPost("/api/job/game/accept", (JobRequest jobRequest, HttpContext context, IJobGameService jobGameService) =>
        {
            var userIdStr = context.User.FindFirst("UserId")?.Value;
            if (!int.TryParse(userIdStr, out var userId))
                return Results.Unauthorized();
            return Results.Ok(jobGameService.AcceptJob(new GameJob(userId, jobRequest.JobId)));
        })
            .WithName("AcceptJob")
            .WithTags("Job Game")
            .WithOpenApi()
            .RequireAuthorization();

        // this endpoint allows the user to retrieve the current game statistics, including the number of accepted and rejected jobs.
        routes.MapPost("/api/job/game/stats", (HttpContext context, IJobGameService jobGameService) =>
        {
            var userIdStr = context.User.FindFirst("UserId")?.Value;
            if (!int.TryParse(userIdStr, out var userId))
                return Results.Unauthorized();
            return Results.Ok(jobGameService.GetGameStats(new CurrentUser(userId)));
        })
            .WithName("GetGameStats")
            .WithTags("Job Game")
            .WithOpenApi()
            .RequireAuthorization();
    }

    public static void MapJobCreationEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapPost("/api/job/add", (NewJob newJob, HttpContext context, IJobAddService jobAddService) =>
        {
            var userId = context.User.FindFirst("UserId")?.Value;
            int posterUserId = int.TryParse(userId, out var id) ? id : 0;
            return jobAddService.AddNewJob(posterUserId, newJob);
        })
            .WithName("AddNewJob")
            .WithTags("Job Creation")
            .WithOpenApi()
            .RequireAuthorization();
    }
}