using System.Collections.Generic;

using ContractEntities = Superkatten.Katministratie.Contract.Entities;

namespace Superkatten.Katministratie.Application.Interfaces;

public interface ISettingsService
{
    IReadOnlyCollection<int> GetCageNumbersForCageAreaAsync(ContractEntities.CatArea contractCatArea);
}
