using Superkatten.Katministratie.Contract.ApiInterface;

namespace Superkatten.Katministratie.Host.Services.Interfaces;

public interface IMedicalProcedureService
{
    Task AddMedicalProcedureAsync(Guid superkatId, AddMedicalProcedureParameters parameters);
    Task<IReadOnlyCollection<MedicalProcedureInformation>> GetAllMedicalProcedures();
}
