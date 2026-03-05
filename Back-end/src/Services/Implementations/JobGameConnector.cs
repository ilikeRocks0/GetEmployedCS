using Back_end.Endpoints.Models;
using Back_end.Persistence.Interfaces;
using Back_end.Persistence.Objects;
using Back_end.Services.Implementations.Finders;
using Back_end.Services.Interfaces;

namespace Back_end.Services.Implementations;

public class JobGameConnector (IUserPersistence userPersistence, IJobPersistence jobPersistence, IJobService jobService) : IJobGameConnector
{
    Dictionary<int, IGameService> GameServiceList = new Dictionary<int, IGameService>(); //mapping of game service to user
    UserFinder userFinder = new UserFinder(userPersistence);
    JobFinder jobFinder = new JobFinder(jobPersistence);

    public Job? InitializeJobGame(CurrentUser currentUser, IReadOnlyDictionary<string, string>? filters = null)
    {
        if (currentUser is null)
        {
            throw new InvalidOperationException("No UserId provided");
        }     
        User? user = userFinder.GetUser(currentUser.UserId);
        if (user is null)
        {
            throw new InvalidOperationException("UserId doesn't match an existing user");
        }
        GameServiceList[user.UserId] = new GameService(new ShuffleJobsService(jobService));
        return GameServiceList[user.UserId].InitializeJobGame();
    }
    
    public Job? RejectJob(GameJob gameJob)
    {
        if (gameJob is null)
        {
            throw new InvalidOperationException("Invalid format for user id and job id provided");
        }     
        User? user = userFinder.GetUser(gameJob.UserId);
        Job? job = jobFinder.GetJob(gameJob.JobId);
        if (user is null || job is null || !GameServiceList.ContainsKey(user.UserId))
        {
            throw new InvalidOperationException("UserId doesn't match an existing user");
        }
        return GameServiceList[user.UserId].RejectJob();  
    }

    public Job? AcceptJob(GameJob gameJob)
    {
        if (gameJob is null)
        {
            throw new InvalidOperationException("Invalid format for user id and job id provided");
        }     
        User? user = userFinder.GetUser(gameJob.UserId);
        Job? job = jobFinder.GetJob(gameJob.JobId);
        if (user is null || job is null || !GameServiceList.ContainsKey(user.UserId))
        {
            throw new InvalidOperationException("UserId doesn't match an existing user");
        }
        //some check for if user already has the job saved, only do the below work if they do NOT have it saved
        userPersistence.SaveJob(user.UserId, job.JobId);
        return GameServiceList[user.UserId].AcceptJob();         
    }

    public (int accepted, int rejected) GetGameStats(CurrentUser currentUser)
    {
        if (currentUser is null)
        {
            throw new InvalidOperationException("No UserId provided");
        }     
        User? user = userFinder.GetUser(currentUser.UserId);
        if (user is null || !GameServiceList.ContainsKey(user.UserId))
        {
            return (-1, -1); //if user doesn't exist or is not in GameServiceList's mapping
        }
        return GameServiceList[user.UserId].GetGameStats();        
    }
}