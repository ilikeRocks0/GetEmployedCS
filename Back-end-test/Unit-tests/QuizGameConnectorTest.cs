using Back_end.Persistence.Interfaces;
using Back_end.Objects;
using Back_end.Services.Interfaces;
using NSubstitute;
using NUnit.Framework;
using Back_end.Services.Implementations;

public class QuizGameConnectorTest
{
    private IQuizGameConnector quizGameConnector;
    private IQuizItemFetcherFactory quizItemFactory;

    User validUser;
    [SetUp]
    public void Setup()
    {
        quizItemFactory = Substitute.For<IQuizItemFetcherFactory>();
        quizGameConnector = new QuizGameConnector(quizItemFactory);
        validUser = QuizGameConnectorData.validUser; 
    }

    [Test]
    public void AnswerQuizNoSessionFoundTest()
    {
        Assert.Throws<InvalidOperationException>(delegate {quizGameConnector.AnswerQuiz(validUser, "random!");});
    }

    [Test]
    public void GetGameStatsNoSessionFoundTest()
    {
        Assert.Throws<InvalidOperationException>(delegate {quizGameConnector.GetGameStats(validUser);});
    }

    [Test]
    public void GetNextQuizNoSessionFoundTest()
    {
        Assert.Throws<InvalidOperationException>(delegate {quizGameConnector.GetNextQuiz(validUser);});
    }

    [Test]
    public void GetNextQuizSessionFoundTest()
    {
        var spyQuizItemFetcher = Substitute.For<IQuizItemFetcher>();
        quizItemFactory.BuildFetcher().Returns(spyQuizItemFetcher);
        spyQuizItemFetcher.GetQuizItem().Returns(QuizGameData.OneQuiz[0], (QuizItem?)null);

        quizGameConnector.InitializeSession(validUser);
        Assert.That(quizGameConnector.GetNextQuiz(validUser), Is.Not.Null);
    }

    [Test]
    public void GetGameStatsResetNextSessionTest()
    {
        var spyQuizItemFetcher = Substitute.For<IQuizItemFetcher>();
        quizItemFactory.BuildFetcher().Returns(spyQuizItemFetcher);
        spyQuizItemFetcher.GetQuizItem().Returns(QuizGameData.OneQuiz[0], (QuizItem?)null);

        quizGameConnector.InitializeSession(validUser);
        QuizGameStats quizGameStats = quizGameConnector.GetGameStats(validUser);
        Assert.That(quizGameStats.Correct, Is.Zero);
        Assert.That(quizGameStats.Incorrect, Is.Zero);
        Assert.That(quizGameStats.Skipped, Is.Zero);

        quizGameConnector.GetNextQuiz(validUser); 
        quizGameConnector.GetNextQuiz(validUser); 

        QuizGameStats quizGameStats2 = quizGameConnector.GetGameStats(validUser);
        Assert.That(quizGameStats2.Correct, Is.Zero);
        Assert.That(quizGameStats2.Incorrect, Is.Zero);
        Assert.That(quizGameStats2.Skipped, Is.EqualTo(1));

        quizGameConnector.InitializeSession(validUser);
        QuizGameStats quizGameStats3 = quizGameConnector.GetGameStats(validUser);
        Assert.That(quizGameStats3.Correct, Is.Zero);
        Assert.That(quizGameStats3.Incorrect, Is.Zero);
        Assert.That(quizGameStats3.Skipped, Is.Zero);
    }
}