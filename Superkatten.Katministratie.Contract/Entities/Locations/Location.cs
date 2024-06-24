namespace Superkatten.Katministratie.Contract.Entities.Locations;

public class Location
{
    public Guid Id { get; init; }
    public string LocationName { get; init; } = string.Empty;
    public LocationType LocationType { get; init; }
    public LocationNaw Naw { get; init; } = null!;
}
