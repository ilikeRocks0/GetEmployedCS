namespace test;

using Back_end.Persistence.Implementations.Adapters.ObjectAdapters;
using Back_end.Persistence.Objects;
using NUnit.Framework;

public class ExperienceObjectAdapterTest
{
    private string validCompanyName = "Mar & Co";
    private string validPositionTitle = "Full-Stack Developer";
    private string validJobDescription = "Designed and implemented a full-stack feature using Vue.js, C#, and custom SQL queries, supporting end-to-end data flow and user interaction.";

    [Test]
    public void HappyCaseTest()
    {
        Experience experience = new Experience(validCompanyName, validPositionTitle, validJobDescription);
        Assert.DoesNotThrow(delegate{ new ExperienceObjectAdapter(experience); });
    }

    [Test]
    public void EmptyCompanyNameTest()
    {
        Experience experience = new Experience("", validPositionTitle,validJobDescription);
        Assert.Throws<ObjectConversionException>(delegate{new ExperienceObjectAdapter(experience);});
    }

    [Test]
    public void SpaceCompanyNameTest()
    {
        Experience experience = new Experience("    ", validPositionTitle,validJobDescription);
        Assert.Throws<ObjectConversionException>(delegate{new ExperienceObjectAdapter(experience);});
    }

    [Test]
    public void EmptyPositionTitleTest()
    {
        Experience experience = new Experience(validCompanyName, "",validJobDescription);
        Assert.Throws<ObjectConversionException>(delegate{new ExperienceObjectAdapter(experience);});
    }

    [Test]
    public void SpacePositionTitleTest()
    {
        Experience experience = new Experience(validCompanyName, "     ",validJobDescription);
        Assert.Throws<ObjectConversionException>(delegate{new ExperienceObjectAdapter(experience);});
    }
}