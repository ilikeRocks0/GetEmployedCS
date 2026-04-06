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

    /// Initialize a job list for the game based on the provided filters. 
    /// <param name="currentUser">The user accessing the game.
    /// Returns a random job to start the game.
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
    
    /// Reject the current job. The game statistics are updated to reflect the rejection.
    /// <param name="gameJob">The job being rejected.
    /// Returns the next job in the game.
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

    /// Accept the current job. The game statistics are updated to reflect the acceptance.    /// <param name="user">The user accessing the game.
    /// <param name="gameJob">The job being accepted.
    /// Returns the next job in the game.
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

    /// Get the current game statistics, including the number of accepted and rejected jobs.
    /// <param name="currentUser">The user accessing the game.
    /// Returns a tuple containing the number of accepted and rejected jobs.
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