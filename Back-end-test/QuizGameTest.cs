using Back_end.Services.Interfaces;
using Back_end.Util;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;

public class QuizGameTest
{
    private IQuizGame quizGame;
    private IQuizItemFetcher quizItemFetcher;

    [SetUp]
    public void Setup()
    {
        quizItemFetcher = Substitute.For<IQuizItemFetcher>();
        quizGame = new QuizGame(quizItemFetcher);
    }

    [Test]
    public void AnswerBeforeInitializationTest()
    {
        Assert.Throws<InvalidOperationException>(delegate{quizGame.AnswerQuiz("Nothing");});
    }

    [Test]
    public void StatsBeforeInitializationTest()
    {
        QuizGameStats quizGameStats = quizGame.GetQuizGameStats();
        Assert.That(quizGameStats.Correct, Is.Zero);
        Assert.That(quizGameStats.Incorrect, Is.Zero);
        Assert.That(quizGameStats.Skipped, Is.Zero);
    }

    [Test]
    public void FetchFromEmptyTest()
    {
        quizItemFetcher.GetQuizItem().ReturnsNull();
        QuizItem? quizItem = quizGame.GetNextQuiz();
        Assert.That(quizItem, Is.Null);
    
    }
    [Test]
    public void FetchOneTest()
    {
        quizItemFetcher.GetQuizItem().Returns(QuizGameData.OneQuiz[0], (QuizItem?)null);
        QuizItem? quizItem1 = quizGame.GetNextQuiz();
        QuizItem? quizItem2 = quizGame.GetNextQuiz();
        Assert.That(quizItem1, Is.EqualTo(QuizGameData.OneQuiz[0]));
        Assert.That(quizItem2, Is.Null);
    }

    [Test]
    public void FetchThreeTest()
    {
        quizItemFetcher.GetQuizItem().Returns(QuizGameData.QuizList[0],QuizGameData.QuizList[1],QuizGameData.QuizList[2],(QuizItem?)null);
        QuizItem? quizItem1 = quizGame.GetNextQuiz();
        QuizItem? quizItem2 = quizGame.GetNextQuiz();
        QuizItem? quizItem3 = quizGame.GetNextQuiz();
        QuizItem? quizItem4 = quizGame.GetNextQuiz();
        Assert.That(quizItem1, Is.EqualTo(QuizGameData.QuizList[0]));
        Assert.That(quizItem2, Is.EqualTo(QuizGameData.QuizList[1]));
        Assert.That(quizItem3, Is.EqualTo(QuizGameData.QuizList[2]));
        Assert.That(quizItem4, Is.Null);
    }

    [Test]
    public void SpamTest()
    {
        quizItemFetcher.GetQuizItem().Returns(QuizGameData.OneQuiz[0],(QuizItem?)null);
        QuizItem? quizItem1 = quizGame.GetNextQuiz();
        QuizItem? quizItem2 = quizGame.GetNextQuiz();
        QuizItem? quizItem3 = quizGame.GetNextQuiz();
        QuizItem? quizItem4 = quizGame.GetNextQuiz();
        QuizItem? quizItem5 = quizGame.GetNextQuiz();
        QuizItem? quizItem6 = quizGame.GetNextQuiz();
        QuizItem? quizItem7 = quizGame.GetNextQuiz();
        Assert.That(quizItem1, Is.EqualTo(QuizGameData.OneQuiz[0]));
        Assert.That(quizItem2, Is.Null);
        Assert.That(quizItem3, Is.Null);
        Assert.That(quizItem4, Is.Null);
        Assert.That(quizItem5, Is.Null);
        Assert.That(quizItem6, Is.Null);
        Assert.That(quizItem7, Is.Null);
    }

    [Test]
    public void SkipOneTest()
    {
        quizItemFetcher.GetQuizItem().Returns(QuizGameData.OneQuiz[0],(QuizItem?)null);
        QuizGameStats quizGameStats = quizGame.GetQuizGameStats();

        Assert.That(quizGameStats.Correct, Is.Zero);
        Assert.That(quizGameStats.Incorrect, Is.Zero);
        Assert.That(quizGameStats.Skipped, Is.Zero);
        quizGame.GetNextQuiz();
        quizGame.GetNextQuiz();

        QuizGameStats quizGameStats2 = quizGame.GetQuizGameStats();
        Assert.That(quizGameStats2.Correct, Is.Zero);
        Assert.That(quizGameStats2.Incorrect, Is.Zero);
        Assert.That(quizGameStats2.Skipped, Is.EqualTo(1));
    }

    [Test]
    public void SkipThreeTest()
    {
        quizItemFetcher.GetQuizItem().Returns(QuizGameData.QuizList[0],QuizGameData.QuizList[1],QuizGameData.QuizList[2],(QuizItem?)null);
        QuizGameStats quizGameStats = quizGame.GetQuizGameStats();

        Assert.That(quizGameStats.Correct, Is.Zero);
        Assert.That(quizGameStats.Incorrect, Is.Zero);
        Assert.That(quizGameStats.Skipped, Is.Zero);
        quizGame.GetNextQuiz();
        quizGame.GetNextQuiz();
        quizGame.GetNextQuiz();
        quizGame.GetNextQuiz();

        QuizGameStats quizGameStats2 = quizGame.GetQuizGameStats();
        Assert.That(quizGameStats2.Correct, Is.Zero);
        Assert.That(quizGameStats2.Incorrect, Is.Zero);
        Assert.That(quizGameStats2.Skipped, Is.EqualTo(3));
    }

    [Test]
    public void SpamSkipTest()
    {
        quizItemFetcher.GetQuizItem().Returns(QuizGameData.OneQuiz[0],(QuizItem?)null);
        QuizGameStats quizGameStats = quizGame.GetQuizGameStats();

        Assert.That(quizGameStats.Correct, Is.Zero);
        Assert.That(quizGameStats.Incorrect, Is.Zero);
        Assert.That(quizGameStats.Skipped, Is.Zero);

        quizGame.GetNextQuiz();
        quizGame.GetNextQuiz();
        quizGame.GetNextQuiz();
        quizGame.GetNextQuiz();

        QuizGameStats quizGameStats2 = quizGame.GetQuizGameStats();
        Assert.That(quizGameStats2.Correct, Is.Zero);
        Assert.That(quizGameStats2.Incorrect, Is.Zero);
        Assert.That(quizGameStats2.Skipped, Is.EqualTo(1));
    }

    [Test]
    public void CorrectAnswerTest()
    {
        quizItemFetcher.GetQuizItem().Returns(QuizGameData.OneQuiz[0],(QuizItem?)null);
        QuizGameStats quizGameStats = quizGame.GetQuizGameStats();

        Assert.That(quizGameStats.Correct, Is.Zero);
        Assert.That(quizGameStats.Incorrect, Is.Zero);
        Assert.That(quizGameStats.Skipped, Is.Zero);

        QuizItem? quizItem1 = quizGame.GetNextQuiz();
        quizGame.AnswerQuiz(quizItem1!.strongSentence);

        QuizGameStats quizGameStats2 = quizGame.GetQuizGameStats();
        Assert.That(quizGameStats2.Correct, Is.EqualTo(1));
        Assert.That(quizGameStats2.Incorrect, Is.Zero);
        Assert.That(quizGameStats2.Skipped, Is.Zero);
    }

    [Test]
    public void IncorrectAnswerTest()
    {
        quizItemFetcher.GetQuizItem().Returns(QuizGameData.OneQuiz[0],(QuizItem?)null);
        QuizGameStats quizGameStats = quizGame.GetQuizGameStats();
        Assert.That(quizGameStats.Correct, Is.Zero);
        Assert.That(quizGameStats.Incorrect, Is.Zero);
        Assert.That(quizGameStats.Skipped, Is.Zero);

        QuizItem? quizItem1 = quizGame.GetNextQuiz();
        quizGame.AnswerQuiz(quizItem1!.weakSentence);

        QuizGameStats quizGameStats2 = quizGame.GetQuizGameStats();
        Assert.That(quizGameStats2.Correct, Is.Zero);
        Assert.That(quizGameStats2.Incorrect, Is.EqualTo(1));
        Assert.That(quizGameStats2.Skipped, Is.Zero);
    }
    
    [Test]
    public void RandomAnswerTest()
    {
        quizItemFetcher.GetQuizItem().Returns(QuizGameData.OneQuiz[0],(QuizItem?)null);
        QuizItem? quizItem1 = quizGame.GetNextQuiz();
        Assert.Throws<InvalidOperationException>(delegate{quizGame.AnswerQuiz("RANDOM!!!");});
    }

    [Test]
    public void SpamRandomAnswerTest()
    {
        quizItemFetcher.GetQuizItem().Returns(QuizGameData.OneQuiz[0],(QuizItem?)null);
        QuizItem? quizItem1 = quizGame.GetNextQuiz();
        Assert.Throws<InvalidOperationException>(delegate{quizGame.AnswerQuiz("RANDOM!!!");});
        Assert.Throws<InvalidOperationException>(delegate{quizGame.AnswerQuiz("RANDOM AGAIN!!!");});
        Assert.Throws<InvalidOperationException>(delegate{quizGame.AnswerQuiz("RANDOM AGAING!!!");});
        Assert.Throws<InvalidOperationException>(delegate{quizGame.AnswerQuiz("I WANT TO WIN!!!");});
    }

    [Test]
    public void DoubleCorrectAnswerTest()
    {
        quizItemFetcher.GetQuizItem().Returns(QuizGameData.OneQuiz[0],(QuizItem?)null);
        QuizItem? quizItem1 = quizGame.GetNextQuiz();
        quizGame.AnswerQuiz(quizItem1!.strongSentence);
        Assert.Throws<InvalidOperationException>(delegate{quizGame.AnswerQuiz(quizItem1.strongSentence);});
    }
    
    [Test]
    public void DoubleIncorrectAnswerTest()
    {
        quizItemFetcher.GetQuizItem().Returns(QuizGameData.OneQuiz[0],(QuizItem?)null);
        QuizItem? quizItem1 = quizGame.GetNextQuiz();
        quizGame.AnswerQuiz(quizItem1!.weakSentence);
        Assert.Throws<InvalidOperationException>(delegate{quizGame.AnswerQuiz(quizItem1.weakSentence);});
    }

    [Test]
    public void DoubleIncorrectCorrectAnswerTest()
    {
        quizItemFetcher.GetQuizItem().Returns(QuizGameData.OneQuiz[0],(QuizItem?)null);
        QuizItem? quizItem1 = quizGame.GetNextQuiz();
        quizGame.AnswerQuiz(quizItem1!.weakSentence);
        Assert.Throws<InvalidOperationException>(delegate{quizGame.AnswerQuiz(quizItem1.strongSentence);});
    }

    [Test]
    public void DoubleCorrectIncorrectAnswerTest()
    {
        quizItemFetcher.GetQuizItem().Returns(QuizGameData.OneQuiz[0],(QuizItem?)null);
        QuizItem? quizItem1 = quizGame.GetNextQuiz();
        quizGame.AnswerQuiz(quizItem1!.strongSentence);
        Assert.Throws<InvalidOperationException>(delegate{quizGame.AnswerQuiz(quizItem1.weakSentence);});
    }

    [Test]
    public void DefaultWinLoseSkipTest()
    {
        quizItemFetcher.GetQuizItem().Returns(QuizGameData.QuizList[0],QuizGameData.QuizList[1],QuizGameData.QuizList[2],(QuizItem?)null);
        QuizGameStats quizGameStats = quizGame.GetQuizGameStats();
        Assert.That(quizGameStats.Correct, Is.Zero);
        Assert.That(quizGameStats.Incorrect, Is.Zero);
        Assert.That(quizGameStats.Skipped, Is.Zero);

        //Win
        QuizItem? quizItem1 = quizGame.GetNextQuiz();
        quizGame.AnswerQuiz(quizItem1!.strongSentence);

        //lose
        QuizItem? quizItem2 = quizGame.GetNextQuiz();
        quizGame.AnswerQuiz(quizItem2!.weakSentence);

        //skip
        quizGame.GetNextQuiz();
        quizGame.GetNextQuiz();

        QuizGameStats quizGameStats2 = quizGame.GetQuizGameStats();
        Assert.That(quizGameStats2.Correct, Is.EqualTo(1));
        Assert.That(quizGameStats2.Incorrect, Is.EqualTo(1));
        Assert.That(quizGameStats2.Skipped, Is.EqualTo(1));
    }

    [Test]
    public void PerfectWinTest()
    {
        quizItemFetcher.GetQuizItem().Returns(QuizGameData.QuizList[0],QuizGameData.QuizList[1],QuizGameData.QuizList[2], null);
        QuizGameStats quizGameStats = quizGame.GetQuizGameStats();
        Assert.That(quizGameStats.Correct, Is.Zero);
        Assert.That(quizGameStats.Incorrect, Is.Zero);
        Assert.That(quizGameStats.Skipped, Is.Zero);

        //Win
        QuizItem? quizItem1 = quizGame.GetNextQuiz();
        quizGame.AnswerQuiz(quizItem1!.strongSentence);

        //lose
        QuizItem? quizItem2 = quizGame.GetNextQuiz();
        quizGame.AnswerQuiz(quizItem2!.strongSentence);

        //skip
        QuizItem? quizItem3 = quizGame.GetNextQuiz();
        quizGame.AnswerQuiz(quizItem3!.strongSentence);

        QuizGameStats quizGameStats2 = quizGame.GetQuizGameStats();
        Assert.That(quizGameStats2.Correct, Is.EqualTo(3));
        Assert.That(quizGameStats2.Incorrect, Is.Zero);
        Assert.That(quizGameStats2.Skipped, Is.Zero);
    }

    [Test]
    public void PerfectLoseTest()
    {
        quizItemFetcher.GetQuizItem().Returns(QuizGameData.QuizList[0],QuizGameData.QuizList[1],QuizGameData.QuizList[2], null);
        QuizGameStats quizGameStats = quizGame.GetQuizGameStats();
        Assert.That(quizGameStats.Correct, Is.Zero);
        Assert.That(quizGameStats.Incorrect, Is.Zero);
        Assert.That(quizGameStats.Skipped, Is.Zero);

        //Win
        QuizItem? quizItem1 = quizGame.GetNextQuiz();
        quizGame.AnswerQuiz(quizItem1!.weakSentence);

        //lose
        QuizItem? quizItem2 = quizGame.GetNextQuiz();
        quizGame.AnswerQuiz(quizItem2!.weakSentence);

        //skip
        QuizItem? quizItem3 = quizGame.GetNextQuiz();
        quizGame.AnswerQuiz(quizItem3!.weakSentence);

        QuizGameStats quizGameStats2 = quizGame.GetQuizGameStats();
        Assert.That(quizGameStats2.Correct, Is.Zero);
        Assert.That(quizGameStats2.Incorrect, Is.EqualTo(3));
        Assert.That(quizGameStats2.Skipped, Is.Zero);
    }
}