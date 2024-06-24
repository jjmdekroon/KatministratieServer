using Superkatten.Katministratie.Application.Interfaces;
using Superkatten.Katministratie.Application.Mappers;
using Superkatten.Katministratie.Contract.Entities;
using Superkatten.Katministratie.Infrastructure.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Superkatten.Katministratie.Application.Services;

public class CatchOriginService : ICatchOriginService
{
    private readonly ICatchOriginRepository _catchOriginRepository;
    private readonly ICatchOriginMapper _catchOriginMapper;

    public CatchOriginService(
        ICatchOriginRepository catchOriginRepository,
        ICatchOriginMapper catchOriginMapper
    )
    {
        _catchOriginRepository = catchOriginRepository;
        _catchOriginMapper = catchOriginMapper;
    }

    public async Task<IReadOnlyCollection<CatchOrigin>> GetCatchOriginsAsync()
    {
        var catchOrigin = await _catchOriginRepository.GetCatchOriginsAsync();

        return catchOrigin
            .Select(_catchOriginMapper.MapDomainToContract)
            .ToList();
    }
}
