using Back_end.Persistence.Interfaces;
using Back_end.Persistence.Objects;
using Back_end.Services.Interfaces;

namespace Back_end.Services.Implementations;

public class JobGameConnector (IUserPersistence userPersistence, IJobService jobService) : IJobGameConnector
{
    Dictionary<int, IGameService> GameServiceList = new Dictionary<int, IGameService>(); //mapping of game service to user
    public Job? InitializeJobGame(User currentUser, IReadOnlyDictionary<string, string>? filters = null)
    {
        GameServiceList[currentUser.UserId] = new GameService(new ShuffleJobsService(jobService));
        return GameServiceList[currentUser.UserId].InitializeJobGame();
    }
    
    public Job? RejectJob(User user, Job job)
    {
        return GameServiceList[user.UserId].RejectJob();  
    }

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

    public (int accepted, int rejected) GetGameStats(User user)
    {
        if (!GameServiceList.ContainsKey(user.UserId))
        {
            throw new InvalidOperationException("UserId doesn't match an existing user");
        }
        return GameServiceList[user.UserId].GetGameStats();        
    }
}