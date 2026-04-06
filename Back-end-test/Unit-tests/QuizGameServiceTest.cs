using Back_end.Endpoints.Models;
using Back_end.Persistence.Interfaces;
using Back_end.Util;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;
using Back_end.Services.Interfaces;
using Back_end.Services.Implementations;

public class QuizGameServiceTest
{
    IQuizGameService quizGameService;
    IUserPersistence userPersistence;
    IQuizItemsPersistence quizItemsPersistence;
    CurrentUser validUser = new CurrentUser(1);
    CurrentUser invalidUser = new CurrentUser(-1);

    [SetUp]
    public void Setup()
    {
        userPersistence = Substitute.For<IUserPersistence>();
        quizItemsPersistence = Substitute.For<IQuizItemsPersistence>();
        quizGameService = new QuizGameService(userPersistence, quizItemsPersistence);
    }

    [Test]
    public void EmptyAnswerTest()
    {
        QuizGameResponse quizGameResponse = new QuizGameResponse("");
        Assert.Throws<InvalidOperationException>(delegate {quizGameService.AnswerQuiz(validUser, quizGameResponse);});
    } 

    [Test]
    public void SpaceAnswerTest()
    {
        QuizGameResponse quizGameResponse = new QuizGameResponse("    ");
        Assert.Throws<InvalidOperationException>(delegate {quizGameService.AnswerQuiz(validUser, quizGameResponse);});
    } 
    
    [Test]
    public void AnswerQuizInvalidUserTest()
    {
        QuizGameResponse quizGameResponse = new QuizGameResponse("I created an RPG over the summer");
        Assert.Throws<InvalidOperationException>(delegate {quizGameService.AnswerQuiz(invalidUser, quizGameResponse);});
    } 

    [Test]    
    public void InitializeSessionInvalidUserTest()
    {
        Assert.Throws<InvalidOperationException>(delegate {quizGameService.InitializeSession(invalidUser);});
    } 
    
    [Test]
    public void GetNextQuizInvalidUserTest()
    {
        Assert.Throws<InvalidOperationException>(delegate {quizGameService.GetNextQuiz(invalidUser);});
    } 

    [Test]
    public void GetGameStatsInvalidUserTest()
    {
        Assert.Throws<InvalidOperationException>(delegate {quizGameService.GetGameStats(invalidUser);});
    } 

    [Test]
    public void UserDoesntExistTest()
    {
        userPersistence.GetUser(2).ReturnsNull();
        CurrentUser missingUser = new CurrentUser(2);
        Assert.Throws<InvalidOperationException>(delegate {quizGameService.GetGameStats(invalidUser);});
    } 

    [Test]
    public void UserDoesExistTest()
    {
        userPersistence.GetUser(1).Returns(QuizGameServiceData.user1);
        quizItemsPersistence.GetQuizItems(AppConfig.QUIZ_ITEM_AMOUNT).Returns([]);
        CurrentUser existingUser = new CurrentUser(1);
        Assert.DoesNotThrow(delegate {quizGameService.InitializeSession(existingUser);});
    } 
}