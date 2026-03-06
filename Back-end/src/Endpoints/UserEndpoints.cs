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

        routes.MapPost("/api/users/save", (HttpContext context, IUserService userService) =>
        {
            var filters = context.Request.Query.ToDictionary(query => query.Key, query => query.Value.ToString());
            return userService.SaveJob(filters.Count > 0 ? filters : null);
        })
            .WithName("SaveJob")
            .WithTags("Users")
            .WithOpenApi();

        routes.MapPost("/api/users/{userId}", (int userId, IUserService userService) =>
        {          

            return userService.GetProfile(userId); 
        })
            .WithName("GetProfile")
            .WithTags("Users")
            .WithOpenApi();
    }
}