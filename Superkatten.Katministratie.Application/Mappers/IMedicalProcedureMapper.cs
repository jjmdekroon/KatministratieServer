using Superkatten.Katministratie.Contract.ApiInterface;
using Superkatten.Katministratie.Domain.Entities;
using System;

namespace Superkatten.Katministratie.Application.Mappers;

public interface IMedicalProcedureMapper
{
    MedicalProcedureInformation MapToContract(Superkat superkat, MedicalProcedure medicalProcedure);
    MedicalProcedure MapToDomain(AddMedicalProcedureParameters parameters);
}
