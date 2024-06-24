using Superkatten.Katministratie.Application.Interfaces;
using Superkatten.Katministratie.Application.Mappers;
using Superkatten.Katministratie.Domain.Entities.Locations;
using System.Collections.Generic;

using ContractEntities = Superkatten.Katministratie.Contract.Entities;

namespace Superkatten.Katministratie.Application.Services;

public class SettingsService : ISettingsService
{
    private readonly ISuperkatMapper _mapper;

    public SettingsService(ISuperkatMapper mapper)
    {
        _mapper = mapper;
    }

    public IReadOnlyCollection<int> GetCageNumbersForCageAreaAsync(ContractEntities.CatArea contractCatArea)
    {
        var catArea = _mapper.MapContractToDomain(contractCatArea);
        return Refuge.GetCageNumbersForCatArea(catArea);
    }
}
