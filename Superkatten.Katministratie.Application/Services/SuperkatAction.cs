using Superkatten.Katministratie.Application.CageCard;
using Superkatten.Katministratie.Application.Interfaces;
using Superkatten.Katministratie.Contract.ApiInterface;
using Superkatten.Katministratie.Infrastructure.Interfaces;
using System;
using System.Threading.Tasks;

namespace Superkatten.Katministratie.Application.Services;

public class SuperkatAction : ISuperkatAction
{
    public readonly ISuperkattenRepository SuperkattenRepository;

    public SuperkatAction(ISuperkattenRepository superkattenRepository)
    {
        SuperkattenRepository = superkattenRepository;
    }

    public async Task ToggleRetourAsync(Guid id)
    {
        var superkat = await SuperkattenRepository.GetSuperkatAsync(id);            
        superkat.SetRetour(!superkat.Retour);
        await SuperkattenRepository.UpdateSuperkatAsync(superkat);
    }

    public async Task ToggleReserveAsync(Guid id)
    {
        var superkat = await SuperkattenRepository.GetSuperkatAsync(id);
        superkat.SetReserved(!superkat.Reserved);
        await SuperkattenRepository.UpdateSuperkatAsync(superkat);
    }
}
