using Superkatten.Katministratie.Domain.Exceptions;

namespace Superkatten.Katministratie.Domain.Entities.Locations;

public class Adoptant : BaseLocation
{
    public override LocationType LocationType => LocationType.Adopter;

    public Adoptant()
    {
        // Mandatory for EF
    }

    public Adoptant(string name, string? address, string? postcode, string? city, string? phone, string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            throw new DomainException("Email address may not be empty");
        }

        UpdateNaw(name, address, postcode, city, phone, email);
    }
}
