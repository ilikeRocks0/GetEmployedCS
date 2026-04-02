using Back_end.Persistence.Exceptions;
using Back_end.Persistence.Implementations.Adapters.EntityAdapters;
using Back_end.Persistence.Model;
using NUnit.Framework;

public class QuizItemAdapterTest
{
    private string validWeakSentence = "Helped with customer service at a retail store";
    private string validStrongSentence = "Resolved 40+ customer inquiries daily at a high-volume retail location, maintaining a 95% satisfaction rating";

    [Test]
    public void HappyCaseTest()
    {
        QuizItemEntity quizItemEntity = new QuizItemEntity
        {
            strong_sentence = validStrongSentence, 
            weak_sentence = validWeakSentence
        };
        Assert.DoesNotThrow(delegate{new QuizItemEntityAdapter(quizItemEntity);});
    }

    [Test]
    public void EmptyWeakTest()
    {
        QuizItemEntity quizItemEntity = new QuizItemEntity
        {
            strong_sentence = validStrongSentence, 
            weak_sentence = ""
        };
        Assert.Throws<ObjectConversionException>(delegate{new QuizItemEntityAdapter(quizItemEntity);});
    }
    
    [Test]
    public void SpaceWeakTest()
    {
        QuizItemEntity quizItemEntity = new QuizItemEntity
        {
            strong_sentence = validStrongSentence, 
            weak_sentence = "    "
        };
        Assert.Throws<ObjectConversionException>(delegate{new QuizItemEntityAdapter(quizItemEntity);});
    }

    [Test]
    public void EmptyStrongTest()
    {
        QuizItemEntity quizItemEntity = new QuizItemEntity
        {
            strong_sentence = "", 
            weak_sentence = validWeakSentence,
        };
        Assert.Throws<ObjectConversionException>(delegate{new QuizItemEntityAdapter(quizItemEntity);});
    }
    
    [Test]
    public void SpaceStrongTest()
    {
        QuizItemEntity quizItemEntity = new QuizItemEntity
        {
            strong_sentence = "    ", 
            weak_sentence = validStrongSentence
        };
        Assert.Throws<ObjectConversionException>(delegate{new QuizItemEntityAdapter(quizItemEntity);});
    }
}