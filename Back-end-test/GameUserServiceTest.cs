namespace test;

using Back_end.Objects;
using Back_end.Services.Implementations;
using Back_end.Services.Interfaces;
using NSubstitute;
using NUnit.Framework;

public class GameUserServiceTest
{
    private IUserSwipeGameService gameService;
    private IUserIndexManager userIndexManager;
    
    [SetUp]
    public void Setup()
    {   
        userIndexManager = Substitute.For<IUserIndexManager>();
        gameService = new UserSwipeGameService(userIndexManager);
    }

    [Test]
    public void RejectBeforeInitializationTest()
    {
        userIndexManager.GetUsers().Returns(GameUserServiceData.UserList.ToList(), GameUserServiceData.Empty.ToList());
        Assert.Throws<InvalidOperationException>(delegate {gameService.RejectUser();});
        Assert.Throws<InvalidOperationException>(delegate {gameService.RejectUser();}, "Game not initialized. Please call InitializeUserGame() before  rejecting jobs.");
    }

    [Test]
    public void AcceptBeforeInitializationTest()
    {
        userIndexManager.GetUsers().Returns(GameUserServiceData.UserList.ToList(), GameUserServiceData.Empty.ToList());
        Assert.Throws<InvalidOperationException>(delegate {gameService.AcceptUser();});
        Assert.Throws<InvalidOperationException>(delegate {gameService.AcceptUser();}, "Game not initialized. Please call InitializeUserGame() before accepting jobs.");

    }

    [Test]
    public void InitializeEmptyTest()
    {
        userIndexManager.GetUsers().Returns(GameUserServiceData.Empty.ToList());
        User? job = gameService.InitializeUserGame();
        Assert.That(job, Is.Null);
    }

    [Test]
    public void RejectEmptyTest()
    {
        userIndexManager.GetUsers().Returns(GameUserServiceData.Empty.ToList());
        User? job = gameService.InitializeUserGame();
        using (Assert.EnterMultipleScope())
        {
            Assert.That(job, Is.Null);
            Assert.That(gameService.RejectUser(), Is.Null);
        }

    }

    [Test]
    public void AcceptEmptyTest()
    {
        userIndexManager.GetUsers().Returns(GameUserServiceData.Empty.ToList());
        User? job = gameService.InitializeUserGame();
        using (Assert.EnterMultipleScope())
        {
            Assert.That(job, Is.Null);
            Assert.That(gameService.AcceptUser(), Is.Null);
        }
    }

    [Test]
    public void RejectOneTest()
    {
        userIndexManager.GetUsers().Returns(GameUserServiceData.OneJob.ToList(), GameUserServiceData.Empty.ToList());
        User? job = gameService.InitializeUserGame();
        using (Assert.EnterMultipleScope())
        {
            Assert.That(job, Is.Not.Null);
            Assert.That(gameService.RejectUser(), Is.Null);
        }

    }

    [Test]
    public void AcceptOneTest()
    {
        userIndexManager.GetUsers().Returns(GameUserServiceData.OneJob.ToList(), GameUserServiceData.Empty.ToList());
        User? job = gameService.InitializeUserGame();
        using (Assert.EnterMultipleScope())
        {
            Assert.That(job, Is.Not.Null);
            Assert.That(gameService.AcceptUser(), Is.Null);
        }

    }

    
    [Test]
    public void RejectOneGameStatTest()
    {
        userIndexManager.GetUsers().Returns(GameUserServiceData.OneJob.ToList(), GameUserServiceData.Empty.ToList());
        gameService.InitializeUserGame();
        gameService.RejectUser();
        var (accepted,  rejected) = gameService.GetGameStats();
        Assert.That(accepted, Is.EqualTo(0));
        Assert.That(rejected, Is.EqualTo(1));
    }

    [Test]
    public void AcceptOneGameStatTest()
    {
        userIndexManager.GetUsers().Returns(GameUserServiceData.OneJob.ToList(), GameUserServiceData.Empty.ToList());
        gameService.InitializeUserGame();
        gameService.AcceptUser();

        var (accepted,  rejected) = gameService.GetGameStats();
        Assert.That(accepted, Is.EqualTo(1));
        Assert.That(rejected, Is.Zero);
    }
    
    [Test]
    public void GetMultipleJobTest()
    {
        User job = GameUserServiceData.UserList[0];
        userIndexManager.GetUsers().Returns(GameUserServiceData.UserList.ToList(), GameUserServiceData.Empty.ToList());
        User? job1 = gameService.InitializeUserGame();
        User? job2 = gameService.AcceptUser();
        User? job3 = gameService.AcceptUser();
        User? job4 = gameService.AcceptUser();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(job1, Is.EqualTo(GameUserServiceData.UserList[0]));
            Assert.That(job2, Is.EqualTo(GameUserServiceData.UserList[1]));
            Assert.That(job3, Is.EqualTo(GameUserServiceData.UserList[2]));
            Assert.That(job4, Is.Null);
        }

    }

    [Test]
    public void GetMultipleAcceptGameStatsTest()
    {
        userIndexManager.GetUsers().Returns(GameUserServiceData.UserList.ToList(), GameUserServiceData.Empty.ToList());
        gameService.InitializeUserGame();
        gameService.AcceptUser();
        gameService.AcceptUser();
        gameService.AcceptUser();

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
        userIndexManager.GetUsers().Returns(GameUserServiceData.UserList.ToList(), GameUserServiceData.Empty.ToList());
        gameService.InitializeUserGame();
        gameService.RejectUser();
        gameService.RejectUser();
        gameService.RejectUser();

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
        userIndexManager.GetUsers().Returns(GameUserServiceData.UserList.ToList(), GameUserServiceData.Empty.ToList());
        gameService.InitializeUserGame();
        gameService.AcceptUser();
        gameService.RejectUser();
        gameService.RejectUser();

        var (accepted,  rejected) = gameService.GetGameStats();
        Assert.That(accepted, Is.EqualTo(1));
        Assert.That(rejected, Is.EqualTo(2));
    }

    [Test]
    public void FetchJobsAgainTest()
    {
        userIndexManager.GetUsers().Returns(GameUserServiceData.UserList.ToList(), GameUserServiceData.OneJob.ToList(), GameUserServiceData.Empty.ToList());
        User? job1 = gameService.InitializeUserGame();
        User? job2 = gameService.AcceptUser();
        User? job3 = gameService.AcceptUser();
        User? job4 = gameService.AcceptUser();
        User? job5 = gameService.AcceptUser();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(job1, Is.EqualTo(GameUserServiceData.UserList[0]));
            Assert.That(job2, Is.EqualTo(GameUserServiceData.UserList[1]));
            Assert.That(job3, Is.EqualTo(GameUserServiceData.UserList[2]));
            Assert.That(job4, Is.EqualTo(GameUserServiceData.OneJob[0]));
            Assert.That(job5, Is.Null);
        }

    }

    [Test]
    public void SpamAcceptGameStatsTest()
    {
        userIndexManager.GetUsers().Returns(GameUserServiceData.OneJob.ToList());
        User? job1 = gameService.InitializeUserGame();
        User? job2 = gameService.AcceptUser();
        User? job3 = gameService.AcceptUser();
        User? job4 = gameService.AcceptUser();
        User? job5 = gameService.AcceptUser();

        var (accepted,  rejected) = gameService.GetGameStats();
        Assert.That(accepted, Is.EqualTo(1));
        Assert.That(rejected, Is.Zero);
    }

    [Test]
    public void SpamRejectGameStatsTest()
    {
        userIndexManager.GetUsers().Returns(GameUserServiceData.OneJob.ToList());
        User? job1 = gameService.InitializeUserGame();
        User? job2 = gameService.RejectUser();
        User? job3 = gameService.RejectUser();
        User? job4 = gameService.RejectUser();
        User? job5 = gameService.RejectUser();

        var (accepted,  rejected) = gameService.GetGameStats();
        Assert.That(accepted, Is.EqualTo(0));
        Assert.That(rejected, Is.EqualTo(1));
    }

    [Test]
    public void SpamMixGameStatsTest()
    {
        userIndexManager.GetUsers().Returns(GameUserServiceData.OneJob.ToList());
        User? job1 = gameService.InitializeUserGame();
        User? job2 = gameService.AcceptUser();
        User? job3 = gameService.RejectUser();
        User? job4 = gameService.RejectUser();
        User? job5 = gameService.AcceptUser();

        var (accepted,  rejected) = gameService.GetGameStats();
        Assert.That(accepted, Is.EqualTo(1));
        Assert.That(rejected, Is.Zero);
    }

    
}