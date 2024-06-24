using Superkatten.Katministratie.Domain.Entities;
using Superkatten.Katministratie.Domain.Entities.Locations;

namespace Superkatten.Katministratie.Host.Helpers;

public static class LocationDisplayConverter
{
    public static string ConvertLocation(BaseLocation? location)
    {
        if (location is null)
        {
            return string.Empty;
        }

        var locationIdentifier = RefugeLocation(location.LocationType);

        if (location.LocationType is LocationType.Refuge)
        {
            var refuge = (Refuge)location;
            locationIdentifier += $"-{RefugeSubLocation(refuge.CatArea, refuge.CageNumber)}";
        }

        return locationIdentifier;
    }

    private static string RefugeLocation(LocationType locationType)
    {
        return locationType switch
        {
            LocationType.Refuge => "SK",
            LocationType.HostFamily => "GG",
            LocationType.Adopter => "Adopt",
            _ => ""
        };
    }

    private static string RefugeSubLocation(CatArea catArea, int? cageNumber)
    {
        return catArea switch
        {
            CatArea.Quarantine => $"Q-{cageNumber}",
            CatArea.Infirmary => $"AB-{cageNumber}",
            CatArea.SmallEnclosure => $"S-{cageNumber}",
            CatArea.BigEnclosure => $"B-{cageNumber}",
            CatArea.Room2 => $"R2-{cageNumber}",
            _ => ""
        };
    }
}
