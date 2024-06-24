using Superkatten.Katministratie.Host.Entities;

namespace Superkatten.Katministratie.Host.Services;

public interface ISuperkatActionService
{
    Task ToggleReserveSuperkatAsync(Guid superkatId);
    Task ToggleRetourSuperkatAsync(Guid superkatId);
    Task CreateSuperkatCageCardAsync(SuperkatCageCardPrintParameters parameters);
    Task AdoptSuperkatten(Guid gastgezinId, IReadOnlyCollection<Guid> reservedSuperkattenParameters, string name, string email);
}
