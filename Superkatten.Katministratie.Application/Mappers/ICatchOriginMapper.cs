using Superkatten.Katministratie.Domain.Entities;

using ContractEntities = Superkatten.Katministratie.Contract.Entities;

namespace Superkatten.Katministratie.Application.Mappers;

public interface ICatchOriginMapper
{
    public ContractEntities.CatchOrigin MapDomainToContract(CatchOrigin catchOrigin);
    public CatchOrigin MapContractToDomain(ContractEntities.CatchOrigin catchOrigin);
    public CatchOriginType MapContractToDomain(ContractEntities.CatchOriginType catchOriginType);
}
