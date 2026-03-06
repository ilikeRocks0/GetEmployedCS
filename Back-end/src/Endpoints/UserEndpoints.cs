using Back_end.Services.Interfaces;
using Back_end.Endpoints.Models;

namespace Back_end.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapPost("/api/users", (NewUser newUser, IUserService userService) =>
        {
            return userService.CreateUser(newUser);
        })
            .WithName("CreateUser")
            .WithTags("Users")
            .WithOpenApi();

        //TODO: change this to match what it gets from frontend in request
        //TOOD: return 400 error when SaveJob returns -1 (failure)? 
        routes.MapPost("/api/users/save", (HttpContext context, IUserService userService) =>
        {
            var filters = context.Request.Query.ToDictionary(query => query.Key, query => query.Value.ToString());
            return userService.SaveJob(filters.Count > 0 ? filters : null);
        })
            .WithName("SaveJob")
            .WithTags("Users")
            .WithOpenApi();
    }
}