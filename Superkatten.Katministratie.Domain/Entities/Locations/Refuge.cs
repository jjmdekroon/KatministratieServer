using System.Collections.Generic;
using System.ComponentModel;

namespace Superkatten.Katministratie.Domain.Entities.Locations;

public class Refuge : BaseLocation
{
    public override LocationType LocationType => LocationType.Refuge;

    public const string REFUGE_NAME = "Stichting superkatten";
    public const string REFUGE_ADDRESS = "Huigenstraat 49";
    public const string REFUGE_POSTCODE = "4151 CC";
    public const string REFUGE_CITY = "Acquoy";
    public const string REFUGE_PHONE = "";
    public const string REFUGE_EMAIL = "info@superkatten.nl";

    public CatArea CatArea { get; init;} = CatArea.Quarantine;
    public int? CageNumber { get; init; }

    public Refuge()
    {
        // Mandatory for EF
    }

    public Refuge(CatArea catArea, int? cageNumber)
    {
        CatArea = catArea;
        CageNumber = cageNumber;

        UpdateNaw(
            REFUGE_NAME,
            REFUGE_ADDRESS,
            REFUGE_POSTCODE,
            REFUGE_CITY,
            REFUGE_PHONE,
            REFUGE_EMAIL
        );
    }

    public static List<int> GetCageNumbersForCatArea(CatArea catArea)
    {
        return catArea switch
        {
            CatArea.Quarantine => new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 },
            CatArea.Room2 => new List<int> { 13, 14, 15, 16, 17, 18, 19, 20, 21 },
            CatArea.Infirmary => new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 },
            CatArea.SmallEnclosure => new List<int> { 1 },
            CatArea.BigEnclosure => new() { 1 },
            CatArea.HostFamily => new() { 1 },
            _ => throw new InvalidEnumArgumentException(nameof(catArea), (int)catArea, typeof(CatArea))
        };
    }
}
