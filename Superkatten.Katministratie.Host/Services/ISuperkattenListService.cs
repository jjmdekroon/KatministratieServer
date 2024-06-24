using Superkatten.Katministratie.Contract.ApiInterface;
using Superkatten.Katministratie.Contract.ApiInterface.Reallocate;
using Superkatten.Katministratie.Contract.ApiInterface.Reporting;
using Superkatten.Katministratie.Contract.Entities;

namespace Superkatten.Katministratie.Host.Services
{
    public interface ISuperkattenListService
    {
        Task<List<Superkat>> GetAllSuperkattenAsync();
        Task<List<Superkat>> GetAllNotAssignedSuperkattenAsync();
        Task<List<Superkat>> GetAllNotNeutralizedSuperkattenAsync();
        Task<Superkat> GetSuperkatAsync(Guid id);
        Task<Superkat?> CreateSuperkatAsync(CreateSuperkatParameters newSuperkat);
        Task ReallocateSuperkatAsync(ReallocateInRefugeParameters parameters);
        Task ReallocateSuperkatAsync(Guid id, ReallocateToGastgezinParameters updateSuperkat);
        Task UpdateSuperkatAsync(Guid id, UpdateSuperkatParameters updateSuperkat);
        Task DeleteSuperkatAsync(Guid id);
        Task<Superkat?> UpdateSuperkatPhoto(Guid id, PhotoParameters updateSuperkatPhotoParameters);
    }
}
