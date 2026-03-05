namespace test;

using Back_end.Persistence.Implementations.Adapters.EntityAdapters;
using Back_end.Persistence.Model;
using NUnit.Framework;

//testing input for Experience Entity Adapter
public class ExperienceEntityAdapterTest
{
    private string validCompanyName = "Mar & Co";
    private string validPositionTitle = "Full-Stack Developer";
    private string validJobDescription = "Designed and implemented a full-stack feature using Vue.js, C#, and custom SQL queries, supporting end-to-end data flow and user interaction.";

    [Test]
    public void HappyCaseTest()
    {
        ExperienceEntity experienceEntity = new ExperienceEntity(validCompanyName,validPositionTitle,validJobDescription);
        Assert.DoesNotThrow(delegate{new ExperienceEntityAdapter(experienceEntity);});
    }

    [Test]
    public void EmptyCompanyNameTest()
    {
        ExperienceEntity experienceEntity = new ExperienceEntity("", validPositionTitle,validJobDescription);
        Assert.Throws<ObjectConversionException>(delegate{new ExperienceEntityAdapter(experienceEntity);});
    }

    [Test]
    public void SpaceCompanyNameTest()
    {
        ExperienceEntity experienceEntity = new ExperienceEntity("    ", validPositionTitle,validJobDescription);
        Assert.Throws<ObjectConversionException>(delegate{new ExperienceEntityAdapter(experienceEntity);});
    }

    [Test]
    public void EmptyPositionTitleTest()
    {
        ExperienceEntity experienceEntity = new ExperienceEntity(validCompanyName, "",validJobDescription);
        Assert.Throws<ObjectConversionException>(delegate{new ExperienceEntityAdapter(experienceEntity);});
    }

    [Test]
    public void SpacePositionTitleTest()
    {
        ExperienceEntity experienceEntity = new ExperienceEntity(validCompanyName, "     ",validJobDescription);
        Assert.Throws<ObjectConversionException>(delegate{new ExperienceEntityAdapter(experienceEntity);});
    }
}