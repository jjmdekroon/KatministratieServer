using Superkatten.Katministratie.Contract.ApiInterface;
using Superkatten.Katministratie.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Superkatten.Katministratie.Infrastructure.Interfaces;

public interface IMedicalProceduresRepository
{
    Task<IReadOnlyCollection<MedicalProcedure>> GetAllMedicalProcedureAsync();

    Task AddMedicalProcedureAsync(MedicalProcedure medicalProcedure);
}
