using Superkatten.Katministratie.Domain.Entities;
using Superkatten.Katministratie.Domain.Entities.Locations;
using Superkatten.Katministratie.Host.Helpers;
using System.ComponentModel;

using ContractEntities = Superkatten.Katministratie.Contract.Entities;

namespace Superkatten.Katministratie.Application.Mappers;

public class LocationMapper : ILocationMapper
{
    public ContractEntities.Locations.Location ToContract(BaseLocation location)
    {
        return location.LocationType switch
        {
            LocationType.HostFamily => CreateLocation(location),
            LocationType.Adopter => CreateLocation(location),
            LocationType.Refuge => CreateRefuge((Refuge)location),
            _ => throw new InvalidEnumArgumentException(nameof(location.LocationType), (int)location.LocationType, typeof(LocationType))
        };
    }

    private static ContractEntities.Locations.Location CreateRefuge(Refuge location)
    {
        return new ContractEntities.Locations.Refuge
        {
            Id = location.Id,
            LocationType = ToContract(location.LocationType),
            Naw = ToContract(location.LocationNaw),
            CatArea = ToContract(location.CatArea),
            CageNumber = location.CageNumber,
            LocationName = LocationDisplayConverter.ConvertLocation(location)
        };
    }

    private static ContractEntities.Locations.Location CreateLocation(BaseLocation location)
    {
        return new ContractEntities.Locations.Location
        {
            Id = location.Id,
            LocationType = ToContract(location.LocationType),
            Naw = ToContract(location.LocationNaw),
            LocationName = LocationDisplayConverter.ConvertLocation(location)
        };
    }

    private static ContractEntities.Locations.LocationType ToContract(LocationType locationType)
    {
        return locationType switch
        {
            LocationType.HostFamily => ContractEntities.Locations.LocationType.HostFamily,
            LocationType.Refuge => ContractEntities.Locations.LocationType.Refuge,
            LocationType.Adopter => ContractEntities.Locations.LocationType.Adopter,
            _ => throw new InvalidEnumArgumentException(nameof(locationType), (int)locationType, typeof(LocationType))
        };
    }

    private static ContractEntities.Locations.LocationNaw ToContract(LocationNaw locationNaw)
    {
        return new ContractEntities.Locations.LocationNaw
        {
            Name = locationNaw.Name,
            Address = locationNaw.Address,
            Postcode = locationNaw.Postcode,
            City = locationNaw.City,
            Phone = locationNaw.Phone,
            Email = locationNaw.Email
        };
    }

    private static ContractEntities.CatArea ToContract(CatArea catArea)
    {
        return catArea switch
        {
            CatArea.Quarantine => ContractEntities.CatArea.Quarantine,
            CatArea.Infirmary => ContractEntities.CatArea.Infirmary,
            CatArea.SmallEnclosure => ContractEntities.CatArea.SmallEnclosure,
            CatArea.BigEnclosure => ContractEntities.CatArea.BigEnclosure,
            CatArea.Room2 => ContractEntities.CatArea.Room2,
            _ => throw new InvalidEnumArgumentException(nameof(catArea), (int)catArea, typeof(CatArea))
        };
    }
}
