namespace Superkatten.Katministratie.Domain.Entities.Locations;

public class Gastgezin : BaseLocation
{
    public const string TEMP_GASTGEZIN_NAME = "Temp gastgezin naam";
    public override LocationType LocationType => LocationType.HostFamily;

    public Gastgezin()
    {
        // Mandatory for EF
    }

    public Gastgezin(string name, string? address, string? postcode, string? city, string? phone, string? email)
    {
        UpdateNaw(name, address, postcode, city, phone, email);
    }
}
