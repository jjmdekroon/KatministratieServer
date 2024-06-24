using Superkatten.Katministratie.Contract.ApiInterface;
using Superkatten.Katministratie.Contract.ApiInterface.Reallocate;
using Superkatten.Katministratie.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Superkatten.Katministratie.Application.Interfaces
{
    public interface ISuperkattenService
    {
        Task<Superkat> CreateSuperkatAsync(CreateSuperkatParameters createSuperkatDto);        
        Task<Superkat> ReadSuperkatAsync(Guid guid);
        Task<IReadOnlyCollection<Superkat>> ReadAllSuperkattenAsync();
        Task<IReadOnlyCollection<Superkat>> ReadAvailableSuperkattenAsync();
        Task<IReadOnlyCollection<Superkat>> ReadNotNeutralizedSuperkattenAsync();
        Task<Superkat> UpdateSuperkatAsync(Guid guid, UpdateSuperkatParameters updateSuperkatParameters);
        Task<Superkat> ReallocateToGastgezinAsync(Guid guid, ReallocateToGastgezinParameters reallocateSuperkatParameters);
        Task<Superkat> ReallocateInRefugeAsync(ReallocateInRefugeParameters parameters);
        Task<Superkat> UpdateSuperkatAsync(Guid guid, PhotoParameters updateSuperkatPhotoParameters);
        Task DeleteSuperkatAsync(Guid guid);
    }
}