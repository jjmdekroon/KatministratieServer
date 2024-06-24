using Superkatten.Katministratie.Contract.ApiInterface;
using Superkatten.Katministratie.Domain.Entities;
using System;
using System.ComponentModel;
using ContractEntities = Superkatten.Katministratie.Contract.Entities;

namespace Superkatten.Katministratie.Application.Mappers;

public class MedicalProcedureMapper : IMedicalProcedureMapper
{
    public MedicalProcedureInformation MapToContract(Superkat superkat, MedicalProcedure medicalProcedure)
    {
        return new MedicalProcedureInformation
        {
            Id = superkat.Id,
            UniqueNumber = superkat.UniqueNumber,
            ProcedureType = MapMedicalProcedureTypeToContract(medicalProcedure.ProcedureType),
            Remark = medicalProcedure.Remark,
            Timestamp = medicalProcedure.Timestamp
        };
    }

    private static ContractEntities.MedicalProcedureType MapMedicalProcedureTypeToContract(MedicalProcedureType procedureType)
    {
        return procedureType switch
        {
            MedicalProcedureType.Neutralize => ContractEntities.MedicalProcedureType.Neutralize,
            MedicalProcedureType.Stronghold => ContractEntities.MedicalProcedureType.Stronghold,
            MedicalProcedureType.Sickness => ContractEntities.MedicalProcedureType.Sickness,
            MedicalProcedureType.Checkup => ContractEntities.MedicalProcedureType.Checkup,
            MedicalProcedureType.Note => ContractEntities.MedicalProcedureType.Note,
        };
    }

    public MedicalProcedure MapToDomain(AddMedicalProcedureParameters parameters)
    {
        return new MedicalProcedure(
            MapMedicalProcedureTypeToDomain(parameters.ProcedureType),
            parameters.SuperkatId,
            parameters.Timestamp,
            parameters.Remark);
    }

    private static MedicalProcedureType MapMedicalProcedureTypeToDomain(ContractEntities.MedicalProcedureType procedureType)
    {
        return procedureType switch
        {
            ContractEntities.MedicalProcedureType.Stronghold => MedicalProcedureType.Stronghold,
            ContractEntities.MedicalProcedureType.Neutralize => MedicalProcedureType.Neutralize,
            ContractEntities.MedicalProcedureType.Checkup => MedicalProcedureType.Checkup,
            ContractEntities.MedicalProcedureType.Sickness => MedicalProcedureType.Sickness,
            ContractEntities.MedicalProcedureType.Note => MedicalProcedureType.Note,
            _ => throw new InvalidEnumArgumentException(nameof(procedureType), (int)procedureType, typeof(ContractEntities.MedicalProcedureType))
        };
    }
}