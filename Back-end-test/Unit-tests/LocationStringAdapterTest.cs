namespace test;

using Back_end.Persistence.Exceptions;
using Back_end.Persistence.Implementations.Adapters.ObjectAdapters;
using NUnit.Framework;

public class LocationStringAdapterTest
{
    private readonly string validLocationString = "Winnipeg, Manitoba, Canada";

    [Test]
    public void HappyCaseTest()
    {
        Assert.DoesNotThrow(delegate { new LocationStringAdapter(validLocationString); });
    }

    [Test]
    public void EmptyStringTest()
    {
        Assert.Throws<ObjectConversionException>(delegate { new LocationStringAdapter(""); });
    }

    [Test]
    public void SpaceStringTest()
    {
        Assert.Throws<ObjectConversionException>(delegate { new LocationStringAdapter("        "); });
    }

    [Test]
    public void EmptyComponentTest()
    {
        Assert.Throws<ObjectConversionException>(delegate { new LocationStringAdapter("Winnipeg,, Canada"); });
    }

    [Test]
    public void SpaceComponentTest()
    {
        Assert.Throws<ObjectConversionException>(delegate { new LocationStringAdapter("Winnipeg,    , Canada"); });
    }

    [Test]
    public void NoDelimitterTest()
    {
        Assert.Throws<ObjectConversionException>(delegate { new LocationStringAdapter("Winnipeg Manitoba Canada"); });
    }

    [Test]
    public void MissingDelimitterTest()
    {
        Assert.Throws<ObjectConversionException>(delegate { new LocationStringAdapter("Winnipeg Manitoba, Canada"); });
    }

    [Test]
    public void ExtraComponentTest()
    {
        Assert.Throws<ObjectConversionException>(delegate { new LocationStringAdapter("Winnipeg, Manitoba, Canada, North America"); });
    }

    [Test]
    public void MissingComponentTest()
    {
        Assert.Throws<ObjectConversionException>(delegate { new LocationStringAdapter("Winnipeg, Manitoba"); });
    }
}