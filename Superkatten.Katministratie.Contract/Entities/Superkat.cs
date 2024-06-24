using Superkatten.Katministratie.Contract.Entities.Locations;

namespace Superkatten.Katministratie.Contract.Entities;

public class Superkat
{
    public Guid Id { get; init; }
    public int Number { get; init; }
    public SuperkatState State { get; init; }
    public DateTime Birthday { get; init; }
    public DateTime CatchDate { get; init; }
    public CatchOrigin CatchOrigin { get; init; } = new();
    public string Name { get; init; } = string.Empty;
    public bool Reserved { get; init; }
    public bool Retour { get; init; }
    public CatBehaviour Behaviour { get; init; }
    public AgeCategory AgeCategory { get; init; }
    public Gender Gender { get; set; }
    public Location Location { get; init; } = new();
    public string Color { get; init; } = string.Empty;
    public byte[] Photo { get; init; } = Array.Empty<byte>();

    // Volgende moet eigenlijk uit het domain komen en niet hier worden bepaald
    public string UniqueNumber => CatchDate.ToString("yy") + "-" + Number.ToString("000");
}
