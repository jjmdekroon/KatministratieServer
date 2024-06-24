using System;

namespace Superkatten.Katministratie.Domain.Entities;

public class MedicalProcedure
{
    public Guid Id { get; private set; }
    public Guid SuperkatId { get; private set; }
    public DateTime Timestamp { get; private set; }
    public string? Remark { get; private set; }
    public MedicalProcedureType ProcedureType { get; private set; }

    public MedicalProcedure(MedicalProcedureType procedureType, Guid superkatId, DateTime timestamp, string remark)
    {
        Id = Guid.NewGuid();

        ProcedureType = procedureType;
        SuperkatId = superkatId;
        Remark = remark;
        Timestamp = timestamp;
    }
}
