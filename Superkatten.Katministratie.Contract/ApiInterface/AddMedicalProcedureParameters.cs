using Superkatten.Katministratie.Contract.Entities;

namespace Superkatten.Katministratie.Contract.ApiInterface;

public class AddMedicalProcedureParameters
{
    public Guid SuperkatId { get; set; }
    public MedicalProcedureType ProcedureType { get; set; }
    public DateTime Timestamp { get; set; }
    public string Remark { get; set; } = string.Empty;

}
