using Superkatten.Katministratie.Contract.ApiInterface;
using Superkatten.Katministratie.Contract.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Superkatten.Katministratie.Application.Interfaces;

public interface IMedicalProcedureService
{
    Task AddMedicalProcedureAsync(AddMedicalProcedureParameters parameters);
    Task<IReadOnlyCollection<MedicalProcedureInformation>> GetAllMedicalProceduresAsync();
}
