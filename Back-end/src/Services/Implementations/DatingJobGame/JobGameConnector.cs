using Back_end.Persistence.Interfaces;
using Back_end.Objects;
using Back_end.Services.Interfaces;

namespace Back_end.Services.Implementations;

public class JobGameConnector (IUserPersistence userPersistence, IJobService jobService) : IJobGameConnector
{
    Dictionary<int, IGameService> GameServiceList = new Dictionary<int, IGameService>(); //mapping of game service to user

    /// Initialize a job list for the game based on the provided filters. 
    /// <param name="user">The user accessing the game.
    /// <param name="filters">A dictionary of filter keys and values to apply when setting up the game.
    /// - The filters are the same as those used in GetJobs under JobService.
    /// Returns a random job to start the game.
    public Job? InitializeJobGame(User currentUser, IReadOnlyDictionary<string, string>? filters = null)
    {
        GameServiceList[currentUser.UserId] = new GameService(new ShuffleJobsService(jobService));
        return GameServiceList[currentUser.UserId].InitializeJobGame();
    }
    
    /// Reject the current job. The game statistics are updated to reflect the rejection.
    /// <param name="user">The user accessing the game.
    /// <param name="job">The job being rejected. 
    /// Returns the next job in the game.
    public Job? RejectJob(User user, Job job)
    {
        return GameServiceList[user.UserId].RejectJob();  
    }

    /// Accept the current job. The game statistics are updated to reflect the acceptance.    /// <param name="user">The user accessing the game.
    /// <param name="user">The user accessing the game.
    /// <param name="job">The job being accepted. 
    /// Returns the next job in the game.
    public Job? AcceptJob(User user, Job job)
    {
        if (!GameServiceList.ContainsKey(user.UserId))
        {
            throw new InvalidOperationException("UserId doesn't match an existing user");
        }
        
        //If we already saved it just dont save it again
        if (!userPersistence.IsJobInLikes(user.UserId, (int)job.JobId!))
        {
            userPersistence.SaveJob(user.UserId, (int)job.JobId);
        }

        return GameServiceList[user.UserId].AcceptJob();
    }

    /// Get the current game statistics, including the number of accepted and rejected jobs.
    /// Returns a tuple containing the number of accepted and rejected jobs.
    public (int accepted, int rejected) GetGameStats(User user)
    {
        if (!GameServiceList.ContainsKey(user.UserId))
        {
            throw new InvalidOperationException("UserId doesn't match an existing user");
        }
        return GameServiceList[user.UserId].GetGameStats();        
    }
}