using Superkatten.Katministratie.Contract.Entities;

namespace Superkatten.Katministratie.Contract.ApiInterface.Reporting;

public class RequestCageCardEmailParameters
{
    public string Email { get; init; } = string.Empty;
    public CatArea CatArea{ get; init; }
    public int? CageNumber { get; init; }
}
