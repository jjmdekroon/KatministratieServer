namespace Superkatten.Katministratie.Contract.Entities.Locations;

public class LocationNaw
{
    public string Name { get; init; } = null!;
    public string? Address { get; init; }
    public string? Postcode { get; init; }
    public string? City { get; init; }
    public string? Phone { get; init; }
    public string? Email { get; init; }
}
