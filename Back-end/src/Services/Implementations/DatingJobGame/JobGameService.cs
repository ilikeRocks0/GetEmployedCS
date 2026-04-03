using Back_end.Endpoints.Models;
using Back_end.Persistence.Interfaces;
using Back_end.Objects;
using Back_end.Services.Implementations.Finders;
using Back_end.Services.Interfaces;

namespace Back_end.Services.Implementations;

public class JobGameService (IUserPersistence userPersistence, IJobPersistence jobPersistence, IJobService jobService) : IJobGameService
{
    JobGameConnector jobGameConnector = new JobGameConnector(userPersistence, jobService);
    UserFinder userFinder = new UserFinder(userPersistence);
    JobFinder jobFinder = new JobFinder(jobPersistence);

    public Job? InitializeJobGame(CurrentUser currentUser)
    {
        if (currentUser.UserId < 0)
        {
            throw new InvalidOperationException("UserId provided must be non-negative");
        }     
        User? user = userFinder.GetUser(currentUser.UserId);
        if (user is null)
        {
            throw new InvalidOperationException("UserId doesn't match an existing user");
        }
        return jobGameConnector.InitializeJobGame(user);
    }
    
    public Job? RejectJob(GameJob gameJob)
    {
        if (gameJob.UserId < 0)
        {
            throw new InvalidOperationException("UserId provided must be non-negative");
        }
        else if(gameJob.JobId < 0)
        {
            throw new InvalidOperationException("JobId provided must be non-negative");
        }
        User? user = userFinder.GetUser(gameJob.UserId);
        Job? job = jobFinder.GetJob(gameJob.JobId);
        if (user is null || job is null)
        {
            throw new InvalidOperationException("UserId doesn't match an existing user");
        }
        return jobGameConnector.RejectJob(user, job);
    }

    public Job? AcceptJob(GameJob gameJob)
    {
        if (gameJob.UserId < 0)
        {
            throw new InvalidOperationException("UserId provided must be non-negative");
        }
        else if(gameJob.JobId < 0)
        {
            throw new InvalidOperationException("JobId provided must be non-negative");
        }
        User? user = userFinder.GetUser(gameJob.UserId);
        Job? job = jobFinder.GetJob(gameJob.JobId);
        if (user is null || job is null)
        {
            throw new InvalidOperationException("UserId doesn't match an existing user");
        }
        return jobGameConnector.AcceptJob(user, job);
    }

    public (int accepted, int rejected) GetGameStats(CurrentUser currentUser)
    {
        if (currentUser.UserId < 0)
        {
            throw new InvalidOperationException("UserId provided must be non-negative");
        }
        User? user = userFinder.GetUser(currentUser.UserId);
        if (user is null)
        {
            throw new InvalidOperationException("UserId doesn't match an existing user");
        }
        return jobGameConnector.GetGameStats(user);        
    }
}