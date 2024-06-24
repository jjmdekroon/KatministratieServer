using Superkatten.Katministratie.Domain.Entities;

using ContractEntities = Superkatten.Katministratie.Contract.Entities;

namespace Superkatten.Katministratie.Application.Mappers
{
    public interface ISuperkatMapper
    {
        ContractEntities.Superkat MapDomainToContract(Superkat superkat);
        CatArea MapContractToDomain(ContractEntities.CatArea area);
        CatBehaviour MapContractToDomain(ContractEntities.CatBehaviour behaviour);
        Gender MapContractToDomain(ContractEntities.Gender gender);
        LitterGranuleType MapContractToDomain(ContractEntities.LitterGranuleType litterType);
        FoodType MapContractToDomain(ContractEntities.FoodType foodType);
        AgeCategory MapContractToDomain(ContractEntities.AgeCategory ageCategory);
        CatchOrigin MapContractToDomain(ContractEntities.CatchOrigin contractCatchOrigin);
    }
}
