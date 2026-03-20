using System.Diagnostics.CodeAnalysis;
using Back_end.Persistence.Model;

namespace Back_end.Persistence.Implementations.Adapters.ObjectAdapters;

public class LocationStringAdapter : LocationEntity
{
    [SetsRequiredMembers]
    public LocationStringAdapter(string location) : base()
    {
        const int SplitCountryIndex = 2;
        const int SplitStateIndex = 1;
        const int SplitCityIndex = 0;

        string[] splitLocation = location.Split(',', StringSplitOptions.TrimEntries);

        ValidateObject(splitLocation);

        this.country = splitLocation[SplitCountryIndex];
        this.state = splitLocation[SplitStateIndex];
        this.city = splitLocation[SplitCityIndex];
    }

    private void ValidateObject(string[] splitLocation)
    {
        const int ExpectedSplitSize = 3;
        
        if(splitLocation.Length < ExpectedSplitSize)
        {
            throw new ObjectConversionException("Location string must have exactly 3 components.");
        }

        foreach(string s in splitLocation)
        {
            if(s.Equals(String.Empty))
            {
                throw new ObjectConversionException("Location string cannot have empty components.");
            }
        }
    }
}
