using Superkatten.Katministratie.Contract.Entities;

namespace Superkatten.Katministratie.Contract.ApiInterface;

public class MedicalProcedureInformation
{
    public Guid Id { get; init; }
    public string UniqueNumber { get; init; } = null!;
    public DateTime Timestamp { get; init; }
    public string? Remark { get; init; }
    public MedicalProcedureType ProcedureType { get; init; }
}
