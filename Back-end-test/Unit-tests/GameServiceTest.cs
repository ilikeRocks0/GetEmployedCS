namespace test;

using Back_end.Persistence.Objects;
using Back_end.Services.Interfaces;
using NSubstitute;
using NUnit.Framework;

public class GameServiceTest
{
    private IGameService gameService;
    private IJobIndexManager jobIndexManager;
    
    [SetUp]
    public void Setup()
    {   
        jobIndexManager = Substitute.For<IJobIndexManager>();
        gameService = new GameService(jobIndexManager);
    }

    [Test]
    public void RejectBeforeInitializationTest()
    {
        jobIndexManager.GetJobs().Returns(GameServiceData.JobsList.ToList(), GameServiceData.Empty.ToList());
        Assert.Throws<InvalidOperationException>(delegate {gameService.RejectJob();});
        Assert.Throws<InvalidOperationException>(delegate {gameService.RejectJob();}, "Game not initialized. Please call InitializeJobGame() before  rejecting jobs.");
    }

    [Test]
    public void AcceptBeforeInitializationTest()
    {
        jobIndexManager.GetJobs().Returns(GameServiceData.JobsList.ToList(), GameServiceData.Empty.ToList());
        Assert.Throws<InvalidOperationException>(delegate {gameService.AcceptJob();});
        Assert.Throws<InvalidOperationException>(delegate {gameService.AcceptJob();}, "Game not initialized. Please call InitializeJobGame() before accepting jobs.");

    }

    [Test]
    public void InitializeEmptyTest()
    {
        jobIndexManager.GetJobs().Returns(GameServiceData.Empty.ToList());
        Job? job = gameService.InitializeJobGame();
        Assert.That(job, Is.Null);
    }

    [Test]
    public void RejectEmptyTest()
    {
        jobIndexManager.GetJobs().Returns(GameServiceData.Empty.ToList());
        Job? job = gameService.InitializeJobGame();
        using (Assert.EnterMultipleScope())
        {
            Assert.That(job, Is.Null);
            Assert.That(gameService.RejectJob(), Is.Null);
        }

    }

    [Test]
    public void AcceptEmptyTest()
    {
        jobIndexManager.GetJobs().Returns(GameServiceData.Empty.ToList());
        Job? job = gameService.InitializeJobGame();
        using (Assert.EnterMultipleScope())
        {
            Assert.That(job, Is.Null);
            Assert.That(gameService.AcceptJob(), Is.Null);
        }
    }

    [Test]
    public void RejectOneTest()
    {
        jobIndexManager.GetJobs().Returns(GameServiceData.OneJob.ToList(), GameServiceData.Empty.ToList());
        Job? job = gameService.InitializeJobGame();
        using (Assert.EnterMultipleScope())
        {
            Assert.That(job, Is.Not.Null);
            Assert.That(gameService.RejectJob(), Is.Null);
        }

    }

    [Test]
    public void AcceptOneTest()
    {
        jobIndexManager.GetJobs().Returns(GameServiceData.OneJob.ToList(), GameServiceData.Empty.ToList());
        Job? job = gameService.InitializeJobGame();
        using (Assert.EnterMultipleScope())
        {
            Assert.That(job, Is.Not.Null);
            Assert.That(gameService.AcceptJob(), Is.Null);
        }

    }

    
    [Test]
    public void RejectOneGameStatTest()
    {
        jobIndexManager.GetJobs().Returns(GameServiceData.OneJob.ToList(), GameServiceData.Empty.ToList());
        gameService.InitializeJobGame();
        gameService.RejectJob();
        var (accepted,  rejected) = gameService.GetGameStats();
        Assert.That(accepted, Is.EqualTo(0));
        Assert.That(rejected, Is.EqualTo(1));
    }

    [Test]
    public void AcceptOneGameStatTest()
    {
        jobIndexManager.GetJobs().Returns(GameServiceData.OneJob.ToList(), GameServiceData.Empty.ToList());
        gameService.InitializeJobGame();
        gameService.AcceptJob();

        var (accepted,  rejected) = gameService.GetGameStats();
        Assert.That(accepted, Is.EqualTo(1));
        Assert.That(rejected, Is.Zero);
    }
    
    [Test]
    public void GetMultipleJobTest()
    {
        Job job = GameServiceData.JobsList[0];
        jobIndexManager.GetJobs().Returns(GameServiceData.JobsList.ToList(), GameServiceData.Empty.ToList());
        Job? job1 = gameService.InitializeJobGame();
        Job? job2 = gameService.AcceptJob();
        Job? job3 = gameService.AcceptJob();
        Job? job4 = gameService.AcceptJob();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(job1, Is.EqualTo(GameServiceData.JobsList[0]));
            Assert.That(job2, Is.EqualTo(GameServiceData.JobsList[1]));
            Assert.That(job3, Is.EqualTo(GameServiceData.JobsList[2]));
            Assert.That(job4, Is.Null);
        }

    }

    [Test]
    public void GetMultipleAcceptGameStatsTest()
    {
        jobIndexManager.GetJobs().Returns(GameServiceData.JobsList.ToList(), GameServiceData.Empty.ToList());
        gameService.InitializeJobGame();
        gameService.AcceptJob();
        gameService.AcceptJob();
        gameService.AcceptJob();

        var (accepted,  rejected) = gameService.GetGameStats();
        using (Assert.EnterMultipleScope())
        {
            Assert.That(accepted, Is.EqualTo(3));
            Assert.That(rejected, Is.Zero);
        }

    }

    [Test]
    public void GetMultipleFailGameStatsTest()
    {
        jobIndexManager.GetJobs().Returns(GameServiceData.JobsList.ToList(), GameServiceData.Empty.ToList());
        gameService.InitializeJobGame();
        gameService.RejectJob();
        gameService.RejectJob();
        gameService.RejectJob();

        var (accepted,  rejected) = gameService.GetGameStats();
        using (Assert.EnterMultipleScope())
        {
            Assert.That(accepted, Is.Zero);
            Assert.That(rejected, Is.EqualTo(3));
        }

    }

    [Test]
    public void GetMultipleMixGameStatsTest()
    {
        jobIndexManager.GetJobs().Returns(GameServiceData.JobsList.ToList(), GameServiceData.Empty.ToList());
        gameService.InitializeJobGame();
        gameService.AcceptJob();
        gameService.RejectJob();
        gameService.RejectJob();

        var (accepted,  rejected) = gameService.GetGameStats();
        Assert.That(accepted, Is.EqualTo(1));
        Assert.That(rejected, Is.EqualTo(2));
    }

    [Test]
    public void FetchJobsAgainTest()
    {
        jobIndexManager.GetJobs().Returns(GameServiceData.JobsList.ToList(), GameServiceData.OneJob.ToList(), GameServiceData.Empty.ToList());
        Job? job1 = gameService.InitializeJobGame();
        Job? job2 = gameService.AcceptJob();
        Job? job3 = gameService.AcceptJob();
        Job? job4 = gameService.AcceptJob();
        Job? job5 = gameService.AcceptJob();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(job1, Is.EqualTo(GameServiceData.JobsList[0]));
            Assert.That(job2, Is.EqualTo(GameServiceData.JobsList[1]));
            Assert.That(job3, Is.EqualTo(GameServiceData.JobsList[2]));
            Assert.That(job4, Is.EqualTo(GameServiceData.OneJob[0]));
            Assert.That(job5, Is.Null);
        }

    }

    [Test]
    public void SpamAcceptGameStatsTest()
    {
        jobIndexManager.GetJobs().Returns(GameServiceData.OneJob.ToList());
        Job? job1 = gameService.InitializeJobGame();
        Job? job2 = gameService.AcceptJob();
        Job? job3 = gameService.AcceptJob();
        Job? job4 = gameService.AcceptJob();
        Job? job5 = gameService.AcceptJob();

        var (accepted,  rejected) = gameService.GetGameStats();
        Assert.That(accepted, Is.EqualTo(1));
        Assert.That(rejected, Is.Zero);
    }

    [Test]
    public void SpamRejectGameStatsTest()
    {
        jobIndexManager.GetJobs().Returns(GameServiceData.OneJob.ToList());
        Job? job1 = gameService.InitializeJobGame();
        Job? job2 = gameService.RejectJob();
        Job? job3 = gameService.RejectJob();
        Job? job4 = gameService.RejectJob();
        Job? job5 = gameService.RejectJob();

        var (accepted,  rejected) = gameService.GetGameStats();
        Assert.That(accepted, Is.EqualTo(0));
        Assert.That(rejected, Is.EqualTo(1));
    }

    [Test]
    public void SpamMixGameStatsTest()
    {
        jobIndexManager.GetJobs().Returns(GameServiceData.OneJob.ToList());
        Job? job1 = gameService.InitializeJobGame();
        Job? job2 = gameService.AcceptJob();
        Job? job3 = gameService.RejectJob();
        Job? job4 = gameService.RejectJob();
        Job? job5 = gameService.AcceptJob();

        var (accepted,  rejected) = gameService.GetGameStats();
        Assert.That(accepted, Is.EqualTo(1));
        Assert.That(rejected, Is.Zero);
    }

    
}