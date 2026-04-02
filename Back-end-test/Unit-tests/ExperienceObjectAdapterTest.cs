namespace test;

using Back_end.Persistence.Implementations.Adapters.ObjectAdapters;
using Back_end.Objects;
using NUnit.Framework;
using Back_end.Persistence.Exceptions;

public class ExperienceObjectAdapterTest
{
    private string validCompanyName = "Mar & Co";
    private string validPositionTitle = "Full-Stack Developer";
    private string validJobDescription = "Designed and implemented a full-stack feature using Vue.js, C#, and custom SQL queries, supporting end-to-end data flow and user interaction.";

    [Test]
    public void HappyCaseTest()
    {
        Experience experience = new Experience(0, validCompanyName, validPositionTitle, validJobDescription);
        Assert.DoesNotThrow(delegate{ new ExperienceObjectAdapter(experience); });
    }

    [Test]
    public void EmptyCompanyNameTest()
    {
        Experience experience = new Experience(0, "", validPositionTitle, validJobDescription);
        Assert.Throws<ObjectConversionException>(delegate{new ExperienceObjectAdapter(experience);});
    }

    [Test]
    public void SpaceCompanyNameTest()
    {
        Experience experience = new Experience(0, "    ", validPositionTitle, validJobDescription);
        Assert.Throws<ObjectConversionException>(delegate{new ExperienceObjectAdapter(experience);});
    }

    [Test]
    public void EmptyPositionTitleTest()
    {
        Experience experience = new Experience(0, validCompanyName, "", validJobDescription);
        Assert.Throws<ObjectConversionException>(delegate{new ExperienceObjectAdapter(experience);});
    }

    [Test]
    public void SpacePositionTitleTest()
    {
        Experience experience = new Experience(0, validCompanyName, "     ", validJobDescription);
        Assert.Throws<ObjectConversionException>(delegate{new ExperienceObjectAdapter(experience);});
    }
}
