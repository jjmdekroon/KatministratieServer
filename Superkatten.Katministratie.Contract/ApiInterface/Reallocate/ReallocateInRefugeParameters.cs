using Superkatten.Katministratie.Contract.Entities;

namespace Superkatten.Katministratie.Contract.ApiInterface.Reallocate;

public class ReallocateInRefugeParameters
{
    public Guid SuperkatId { get; init; }
    public CatArea CatArea { get; init; }
    public int? CageNumber { get; init; }
}
