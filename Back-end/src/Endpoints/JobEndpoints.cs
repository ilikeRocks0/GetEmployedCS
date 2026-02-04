using Back_end.Services;

namespace Back_end.Endpoints;

public static class JobEndpoints
{
    public static void MapJobEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/jobs", (HttpContext context, IJobService jobService) =>
        {
            var filters = context.Request.Query.ToDictionary(query => query.Key, query => query.Value.ToString());
            return jobService.GetJobs(filters.Count > 0 ? filters : null);
        })
            .WithName("GetJobListings")
            .WithTags("Jobs")
            .WithOpenApi();

        routes.MapGet("/saved-jobs", (HttpContext context, IJobService jobService) =>
        {
            var filters = context.Request.Query.ToDictionary(query => query.Key, query => query.Value.ToString());
            return jobService.GetSavedJobs(filters.Count > 0 ? filters : null);
        })
            .WithName("GetSavedJobs")
            .WithTags("Jobs")
            .WithOpenApi();
    }
}