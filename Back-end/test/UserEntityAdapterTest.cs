namespace test;

using Back_end.Persistence.Implementations.Adapters.EntityAdapters;
using NUnit.Framework;

public class UserEntityAdapterTest
{
    private int validUserId = 1;
    private string validEmail = "kielf@myumanitoba.ca";
    private string validUsername = "DiamondMinecart12";
    private string validPassword = "Password123";
    private string validAboutString = "I am a cool guy! :D";
    private UserEntity userEntity;

    [SetUp]
    public void Setup()
    {   
        userEntity = new()
        {
            user_id = validUserId,
            email = validEmail,
            username = validUsername,
            password = validPassword,
            about_string = validAboutString,
            jobSeeker = new Back_end.Persistence.Model.JobSeekerEntity()
            {
                first_name = "Dan",
                last_name = "TDM"
            }
        };
    }

    [Test]
    public void HappyCaseTest()
    {
        Assert.DoesNotThrow(delegate{new UserEntityAdapter(userEntity);});
    }

    [Test]
    public void InvalidUserIdTest()
    {
        userEntity.user_id = -1;
        Assert.Throws<ObjectConversionException>(delegate{new UserEntityAdapter(userEntity);});
    }

    [Test]
    public void EmptyUsernameTest()
    {
        userEntity.username = "";
        Assert.Throws<ObjectConversionException>(delegate{new UserEntityAdapter(userEntity);});
    }

    [Test]
    public void SpaceUsernameTest()
    {
        userEntity.username = "    ";
        Assert.Throws<ObjectConversionException>(delegate{new UserEntityAdapter(userEntity);});
    }

    [Test]
    public void EmptyEmailTest()
    {
        userEntity.email = "";
        Assert.Throws<ObjectConversionException>(delegate{new UserEntityAdapter(userEntity);});
    }

    [Test]
    public void SpaceEmailTest()
    {
        userEntity.email = "    ";
        Assert.Throws<ObjectConversionException>(delegate{new UserEntityAdapter(userEntity);});
    }

    [Test]
    public void NoAtEmailTest()
    {
        userEntity.email = "thisisjustString";
        Assert.Throws<ObjectConversionException>(delegate{new UserEntityAdapter(userEntity);});
    }

    [Test]
    public void SentenceSpacesEmailTest()
    {
        userEntity.email = "this email @ is not one .com";
        Assert.Throws<ObjectConversionException>(delegate{new UserEntityAdapter(userEntity);});
    }

    [Test]
    public void OnlyAtEmailTest()
    {
        userEntity.email = "@";
        Assert.Throws<ObjectConversionException>(delegate{new UserEntityAdapter(userEntity);});
    }

    [Test]
    public void OnlyDotEmailTest()
    {
        userEntity.email = ".";
        Assert.Throws<ObjectConversionException>(delegate{new UserEntityAdapter(userEntity);});
    }

    [Test]
    public void OnlySpecialEmailTest()
    {
        userEntity.email = "@.";
        Assert.Throws<ObjectConversionException>(delegate{new UserEntityAdapter(userEntity);});
    }

    [Test]
    public void CutOffEmailTest()
    {
        userEntity.email = "was@google.";
        Assert.Throws<ObjectConversionException>(delegate{new UserEntityAdapter(userEntity);});
    }

    [Test]
    public void MissingPrefixEmailTest()
    {
        userEntity.email = "@google.com";
        Assert.Throws<ObjectConversionException>(delegate{new UserEntityAdapter(userEntity);});
    }

    [Test]
    public void MissingCenterEmailTest()
    {
        userEntity.email = "was@.com";
        Assert.Throws<ObjectConversionException>(delegate{new UserEntityAdapter(userEntity);});
    }

    [Test]
    public void WeirdCharactersEmailTest()
    {
        userEntity.email = "!#$%^&*@()[{}]:;<>,.com";
        Assert.Throws<ObjectConversionException>(delegate{new UserEntityAdapter(userEntity);});
    }

    [Test]
    public void EmptyPasswordTest()
    {
        userEntity.password = "";
        Assert.Throws<ObjectConversionException>(delegate{new UserEntityAdapter(userEntity);});
    }

    [Test]
    public void SpacePasswordTest()
    {
        userEntity.password = "    ";
        Assert.Throws<ObjectConversionException>(delegate{new UserEntityAdapter(userEntity);});
    }
}