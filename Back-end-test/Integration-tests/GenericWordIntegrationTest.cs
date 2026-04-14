namespace test;

using Back_end.Persistence.Implementations;
using Back_end.Persistence.Interfaces;
using Back_end.Objects;
using Back_end.Services.Implementations;
using Back_end.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using NUnit.Framework;

public class GenericWordIntegrationTest : IntegrationTest
{
    private IGenericWordsService genericWordsService;
    private IResumePersistence resumePersistence;

    [SetUp]
    public override void Setup()
    {
        base.Setup();
        
        resumePersistence = new ResumePersistence(this.config);
        genericWordsService = new GenericWordsService(resumePersistence);
    }

    [Test]
    public void FollowUserIntegrationTest()
    {
        List<int> index = genericWordsService.GetPositionOfGenericWords("Hello!");
        Assert.That(index, Is.Empty);
    }
}