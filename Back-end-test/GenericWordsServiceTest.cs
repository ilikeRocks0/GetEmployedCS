using Back_end.Services.Implementations;
using NSubstitute;
using Back_end.Persistence.Interfaces;

namespace Tests;

[TestFixture]
public class GenericWordsServiceTest
{
    private GenericWordsService genericWordsService;
    private IResumePersistence resumePersistenceMock;

    [SetUp]
    public void Setup()
    {
        resumePersistenceMock = Substitute.For<IResumePersistence>();        
        genericWordsService = new GenericWordsService(resumePersistenceMock);
    }

    [Test]
    public void TestReturnsCorrectIndices()
    {
        resumePersistenceMock.GetGenericWords().Returns(new List<string> { "apple", "banana" });
        string input = "I eat an apple and a banana"; 

        var result = genericWordsService.GetPositionOfGenericWords(input);

        Assert.That(result, Is.EquivalentTo([3, 6]));
    }

    [Test]
    public void TestIgnorePunctuation()
    {
        resumePersistenceMock.GetGenericWords().Returns(["expert"]);
        string input = "He is an 'expert', right?"; 

        var result = genericWordsService.GetPositionOfGenericWords(input);

        Assert.That(result, Is.EquivalentTo([3]));
    }

    [Test]
    public void TestCaseInsensitive()
    {
        resumePersistenceMock.GetGenericWords().Returns(["Skill"]);
        string input = "skill SKILL Skill"; 

        var result = genericWordsService.GetPositionOfGenericWords(input);

        Assert.That(result, Is.EquivalentTo([0, 1, 2]));
    }

    [Test]
    public void TestNoGenericWords()
    {
        resumePersistenceMock.GetGenericWords().Returns(["nothing"]);
        string input = "This sentence has no generic words.";

        var result = genericWordsService.GetPositionOfGenericWords(input);

        Assert.That(result, Is.Empty);
    }


    [Test]
    public void TestNoGenericWordsinDatabase()
    {
        resumePersistenceMock.GetGenericWords().Returns([]);
        string input = "This sentence has no generic words.";

        var result = genericWordsService.GetPositionOfGenericWords(input);

        Assert.That(result, Is.Empty);
    }

    [Test]
    public void TestEmptyInput()
    {
        resumePersistenceMock.GetGenericWords().Returns(["generic"]);
        string input = "";
        var result = genericWordsService.GetPositionOfGenericWords(input);

        Assert.That(result, Is.Empty);
    }

    [Test]
    public void TestGenericWordsAtStart()
    {
        resumePersistenceMock.GetGenericWords().Returns(["start"]);
        string input = "start middle end";
        var result = genericWordsService.GetPositionOfGenericWords(input);
        Assert.That(result, Is.EquivalentTo([0]));
    }

    [Test]
    public void TestGenericWordsAtEnd()
    {
        resumePersistenceMock.GetGenericWords().Returns(["end"]);
        string input = "start middle end";
        var result = genericWordsService.GetPositionOfGenericWords(input);
        Assert.That(result, Is.EquivalentTo([2]));
    }
    
    [Test]
    public void TestGenericWordsWithPunctuation(){
        resumePersistenceMock.GetGenericWords().Returns(["Don't"]);
        string input = "Don't"; 
        var result = genericWordsService.GetPositionOfGenericWords(input);
        Assert.That(result, Is.EquivalentTo([0]));
    }

    [Test]
    public void TestEmptySpaceinInput(){
        resumePersistenceMock.GetGenericWords().Returns(["Generic"]);
        string input = "      "; 
        var result = genericWordsService.GetPositionOfGenericWords(input);
        Assert.That(result, Is.Empty);
    }

    [Test]
    public void TestLotsOfSpaceinInput(){
        resumePersistenceMock.GetGenericWords().Returns(["Generic"]);
        string input = "   Generic    word   with   lots   of   spaces   "; 
        var result = genericWordsService.GetPositionOfGenericWords(input);
        Assert.That(result, Is.EquivalentTo([0]));
    }

}