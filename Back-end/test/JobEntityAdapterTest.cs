namespace test;

using Back_end.Persistence.Model;
using NUnit.Framework;

public class JobEntityAdapterTest
{  
    private string validJobTitle = "Canada Hydro";
    private string validApplicationLink = "https://linkdon.com/2025";
    private string validPositionType = "Full-Stack";
    private string validEmploymentType = "Internship";
    private string validJobDescription = "Build and maintain user-facing features for our internal energy management dashboard using React and TypeScript.";
    private ICollection<JobLocationEntity> validJobLocations = [];
    private ICollection<LikeEntity> validlikes = [];
    private JobEntity jobEntity;

    [SetUp]
    public void Setup()
    {   
        jobEntity = new()
        {
            job_title = validJobTitle,
            application_link = validApplicationLink,
            position_type = validPositionType,
            employment_type = validEmploymentType,
            job_description = validJobDescription,
            locations = validJobLocations,
            likes = validlikes,
        };
    }

    [Test]
    public void HappyCaseTest()
    {
        Assert.DoesNotThrow(delegate{new JobEntityAdapter(jobEntity);});
    }

    [Test]
    public void EmptyJobTitleTest()
    {
        jobEntity.job_title = "";
        Assert.Throws<ObjectConversionException>(delegate{new JobEntityAdapter(jobEntity);});
    }

    [Test]
    public void SpaceJobTitleTest()
    {
        jobEntity.job_title = "    ";
        Assert.Throws<ObjectConversionException>(delegate{new JobEntityAdapter(jobEntity);});
    }

    [Test]
    public void EmptyApplicationLinkTest()
    {
        jobEntity.application_link = "";
        Assert.Throws<ObjectConversionException>(delegate{new JobEntityAdapter(jobEntity);});
    }

    [Test]
    public void SpaceApplicationLinkTest()
    {
        jobEntity.application_link = "   ";
        Assert.Throws<ObjectConversionException>(delegate{new JobEntityAdapter(jobEntity);});
    }

    [Test]
    public void OnlyDotApplicationLinkTest()
    {
        jobEntity.application_link = ".";
        Assert.Throws<ObjectConversionException>(delegate{new JobEntityAdapter(jobEntity);});
    }

    [Test]
    public void MissingPostfixApplicationLinkTest()
    {
        jobEntity.application_link = ".com";
        Assert.Throws<ObjectConversionException>(delegate{new JobEntityAdapter(jobEntity);});
    }
    
    [Test]
    public void MissingPrefixApplicationLinkTest()
    {
        jobEntity.application_link = "google.";
        Assert.Throws<ObjectConversionException>(delegate{new JobEntityAdapter(jobEntity);});
    }
    
    [Test]
    public void SlashBeforePeriodTest()
    {
        jobEntity.application_link = "linkdon/2025.com";
        Assert.Throws<ObjectConversionException>(delegate{new JobEntityAdapter(jobEntity);});
    }

    [Test]
    public void EmptyPositionTypeTest()
    {
        jobEntity.position_type = "";
        Assert.Throws<ObjectConversionException>(delegate{new JobEntityAdapter(jobEntity);});
    }

    [Test]
    public void SpacePositionTypeTest()
    {
        jobEntity.position_type = "    ";
        Assert.Throws<ObjectConversionException>(delegate{new JobEntityAdapter(jobEntity);});
    }


    [Test]
    public void EmptyEmploymentTypeTest()
    {
        jobEntity.employment_type = "";
        Assert.Throws<ObjectConversionException>(delegate{new JobEntityAdapter(jobEntity);});
    }

    [Test]
    public void SpaceEmploymentTypeTest()
    {
        jobEntity.employment_type = "    ";
        Assert.Throws<ObjectConversionException>(delegate{new JobEntityAdapter(jobEntity);});
    }    

    [Test]
    public void EmptyJobDescriptionTest()
    {
        jobEntity.job_description = "";
        Assert.Throws<ObjectConversionException>(delegate{new JobEntityAdapter(jobEntity);});
    }

    [Test]
    public void SpaceJobDescriptionTest()
    {
        jobEntity.job_description = "    ";
        Assert.Throws<ObjectConversionException>(delegate{new JobEntityAdapter(jobEntity);});
    }    
}