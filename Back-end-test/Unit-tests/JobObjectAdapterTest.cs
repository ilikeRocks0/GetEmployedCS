namespace test;

using Back_end.Persistence.Implementations.Adapters.ObjectAdapters;
using Back_end.Persistence.Objects;
using NUnit.Framework;

public class JobObjectAdapterTest
{  
    private string validJobTitle = "Canada Hydro";
    private string validApplicationLink = "https://linkdon.com/2025";
    private string validPositionType = "Full-Stack";
    private string validEmploymentType = "Internship";
    private string validJobDescription = "Build and maintain user-facing features for our internal energy management dashboard using React and TypeScript.";
    private Job job;

    [SetUp]
    public void Setup()
    {   
        job = new Job(validJobTitle, validApplicationLink, validPositionType, validEmploymentType, validJobDescription);
    }

    [Test]
    public void HappyCaseTest()
    {
        Assert.DoesNotThrow(delegate{new JobObjectAdapter(job);});
    }

    [Test]
    public void EmptyJobTitleTest()
    {
        job.JobTitle = "";
        Assert.Throws<ObjectConversionException>(delegate{new JobObjectAdapter(job);});
    }

    [Test]
    public void SpaceJobTitleTest()
    {
        job.JobTitle = "    ";
        Assert.Throws<ObjectConversionException>(delegate{new JobObjectAdapter(job);});
    }

    [Test]
    public void EmptyApplicationLinkTest()
    {
        job.ApplicationLink = "";
        Assert.Throws<ObjectConversionException>(delegate{new JobObjectAdapter(job);});
    }

    [Test]
    public void SpaceApplicationLinkTest()
    {
        job.ApplicationLink = "   ";
        Assert.Throws<ObjectConversionException>(delegate{new JobObjectAdapter(job);});
    }

    [Test]
    public void OnlyDotApplicationLinkTest()
    {
        job.ApplicationLink = ".";
        Assert.Throws<ObjectConversionException>(delegate{new JobObjectAdapter(job);});
    }

    [Test]
    public void MissingPostfixApplicationLinkTest()
    {
        job.ApplicationLink = ".com";
        Assert.Throws<ObjectConversionException>(delegate{new JobObjectAdapter(job);});
    }
    
    [Test]
    public void MissingPrefixApplicationLinkTest()
    {
        job.ApplicationLink = "google.";
        Assert.Throws<ObjectConversionException>(delegate{new JobObjectAdapter(job);});
    }
    
    [Test]
    public void SlashBeforePeriodTest()
    {
        job.ApplicationLink = "linkdon/2025.com";
        Assert.Throws<ObjectConversionException>(delegate{new JobObjectAdapter(job);});
    }

    [Test]
    public void EmptyPositionTypeTest()
    {
        job.PositionType = "";
        Assert.Throws<ObjectConversionException>(delegate{new JobObjectAdapter(job);});
    }

    [Test]
    public void SpacePositionTypeTest()
    {
        job.PositionType = "    ";
        Assert.Throws<ObjectConversionException>(delegate{new JobObjectAdapter(job);});
    }


    [Test]
    public void EmptyEmploymentTypeTest()
    {
        job.EmploymentType = "";
        Assert.Throws<ObjectConversionException>(delegate{new JobObjectAdapter(job);});
    }

    [Test]
    public void SpaceEmploymentTypeTest()
    {
        job.EmploymentType = "    ";
        Assert.Throws<ObjectConversionException>(delegate{new JobObjectAdapter(job);});
    }    

    [Test]
    public void EmptyJobDescriptionTest()
    {
        job.JobDescription = "";
        Assert.Throws<ObjectConversionException>(delegate{new JobObjectAdapter(job);});
    }

    [Test]
    public void SpaceJobDescriptionTest()
    {
        job.JobDescription = "    ";
        Assert.Throws<ObjectConversionException>(delegate{new JobObjectAdapter(job);});
    }    
}