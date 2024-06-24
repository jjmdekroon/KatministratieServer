namespace Superkatten.Katministratie.Contract.Entities.Locations;

public class Refuge : Location
{
    public CatArea CatArea { get; init; }
    public int? CageNumber { get; init; }
}
