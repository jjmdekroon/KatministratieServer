using Superkatten.Katministratie.Domain.Exceptions;
using System;

namespace Superkatten.Katministratie.Domain.Entities.Locations;

public class LocationNaw
{
    public Guid Id { get; set; }
    public BaseLocation? Location { get; init; }
    public string Name { get; init; } = null!;
    public string? Address { get; init; }
    public string? Postcode { get; init; }
    public string? City { get; init; }
    public string? Phone { get; init; }
    public string? Email { get; init; }

    public static LocationNaw Create(string name, string? address, string? postcode, string? city, string? phone, string? email)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new DomainException("Name may not be empty");
        }

        return new LocationNaw
        {
            Name = name,
            Address = address,
            Postcode = postcode,
            City = city,
            Phone = phone,
            Email = email
        };
    }
}
