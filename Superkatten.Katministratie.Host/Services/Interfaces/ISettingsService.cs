using Superkatten.Katministratie.Contract.Entities;

namespace Superkatten.Katministratie.Host.Services.Interfaces;

public interface ISettingsService
{
    Task<IReadOnlyCollection<int>> GetCageNumbersForCatAreaAsync(CatArea catArea);
}
