using Back_end.Endpoints.Models;
using Back_end.Persistence.Interfaces;
using Back_end.Objects;
using Back_end.Services.Implementations;
using Back_end.Services.Interfaces;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace test;

public class JobGameServiceTest
{
    private IUserPersistence userPersistence;
    private IJobPersistence jobPersistence;
    private IJobService jobService;
    private IJobGameService jobGameService;
    private CurrentUser user;
    private GameJob gameJob;

    [SetUp]
    public void Setup()
    {
        userPersistence = Substitute.For<IUserPersistence>();
        jobPersistence = Substitute.For<IJobPersistence>();
        jobService = Substitute.For<IJobService>();
        user = new CurrentUser(1);
        gameJob = new GameJob(user.UserId, 1);
        jobGameService = new JobGameService(userPersistence, jobPersistence, jobService);
    }

    [Test]
    public void InitializeJobGameNegativeUserId()
    {
        user.UserId = -1;
        Assert.Throws<InvalidOperationException>(delegate{ jobGameService.InitializeJobGame(user); });
    }

    [Test]
    public void InitializeJobGameNullUser()
    {
        userPersistence.GetUser(Arg.Any<int>()).ReturnsNullForAnyArgs();
        Assert.Throws<InvalidOperationException>(delegate{ jobGameService.InitializeJobGame(user); });
    }

    [Test]
    public void InitializeJobGameHappy()
    {
        userPersistence.GetUser(user.UserId).Returns(new User(user.UserId, "email@gmail.com", "user", "pass", "I am cool", "first", "last", []));
        Assert.DoesNotThrow(delegate{ jobGameService.InitializeJobGame(user); });
    }

    [Test]
    public void RejectJobNegativeUserId()
    {
        gameJob.UserId = -1;
        Assert.Throws<InvalidOperationException>(delegate{ jobGameService.RejectJob(gameJob); });
    }

    [Test]
    public void RejectJobNegativeJobId()
    {
        gameJob.JobId = -1;
        Assert.Throws<InvalidOperationException>(delegate{ jobGameService.RejectJob(gameJob); });
    }

    [Test]
    public void RejectJobNullUser()
    {
        userPersistence.GetUser(Arg.Any<int>()).ReturnsNullForAnyArgs();
        Assert.Throws<InvalidOperationException>(delegate{ jobGameService.RejectJob(gameJob); });
    }
    
    [Test]
    public void RejectJobNullJob()
    {
        jobPersistence.GetJobFromJobId(Arg.Any<int>()).ReturnsNullForAnyArgs();
        Assert.Throws<InvalidOperationException>(delegate{ jobGameService.RejectJob(gameJob); });
    }

    [Test]
    public void RejectJobHappy()
    {
        userPersistence.GetUser(gameJob.UserId).Returns(new User(user.UserId, "email@gmail.com", "user", "pass", "I am cool", "first", "last", []));
        jobPersistence.GetJobFromJobId(gameJob.JobId).Returns(new Job("title", "link", "position", "employment", "description"));
        Assert.DoesNotThrow(delegate{ 
            jobGameService.InitializeJobGame(user);
            jobGameService.RejectJob(gameJob); 
        });
    }

    [Test]
    public void AcceptJobNegativeUserId()
    {
        gameJob.UserId = -1;
        Assert.Throws<InvalidOperationException>(delegate{ jobGameService.AcceptJob(gameJob); });
    }

    [Test]
    public void AcceptJobNegativeJobId()
    {
        gameJob.JobId = -1;
        Assert.Throws<InvalidOperationException>(delegate{ jobGameService.AcceptJob(gameJob); });
    }

    [Test]
    public void AcceptJobNullUser()
    {
        userPersistence.GetUser(Arg.Any<int>()).ReturnsNullForAnyArgs();
        Assert.Throws<InvalidOperationException>(delegate{ jobGameService.AcceptJob(gameJob); });
    }
    
    [Test]
    public void AcceptJobNullJob()
    {
        jobPersistence.GetJobFromJobId(Arg.Any<int>()).ReturnsNullForAnyArgs();
        Assert.Throws<InvalidOperationException>(delegate{ jobGameService.AcceptJob(gameJob); });
    }

    [Test]
    public void AcceptJobHappy()
    {
        userPersistence.GetUser(user.UserId).Returns(new User(user.UserId, "email@gmail.com", "user", "pass", "I am cool", "first", "last", []));
        userPersistence.IsJobInLikes(Arg.Any<int>(), Arg.Any<int>()).ReturnsForAnyArgs(true);
        Job job = new Job("title", "link", "position", "employment", "description")
        {
            JobId = gameJob.JobId
        };
        jobPersistence.GetJobFromJobId(gameJob.JobId).Returns(job);
        Assert.DoesNotThrow(delegate{ 
            jobGameService.InitializeJobGame(user);
            jobGameService.AcceptJob(gameJob); 
        });
    }

    [Test]
    public void GetGameStatsNegativeUserId()
    {
        user.UserId = -1;
        Assert.Throws<InvalidOperationException>(delegate{ jobGameService.GetGameStats(user); });
    }

    [Test]
    public void GetGameStatsNullUser()
    {
        userPersistence.GetUser(Arg.Any<int>()).ReturnsNullForAnyArgs();
        Assert.Throws<InvalidOperationException>(delegate{ jobGameService.GetGameStats(user); });
    }

    [Test]
    public void GetGameStatsHappy()
    {
        userPersistence.GetUser(user.UserId).Returns(new User(user.UserId, "email@gmail.com", "user", "pass", "I am cool", "first", "last", []));
        Assert.DoesNotThrow(delegate
        {
            jobGameService.InitializeJobGame(user);
            jobGameService.GetGameStats(user);
        });
    }
}