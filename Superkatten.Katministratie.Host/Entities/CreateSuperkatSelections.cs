using Superkatten.Katministratie.Contract.Entities;

namespace Superkatten.Katministratie.Host.Entities;


public class CreateSuperkatSelections
{
    public CatchOriginType CatchOriginType { get; private set; } = CatchOriginType.PrivateProperty;
    public CatBehaviour CatBehaviour { get; private set; } = CatBehaviour.Shy;
    public AgeCategory AgeCategory { get; private set; } = AgeCategory.Kitten;
    public CatArea CatArea { get; private set; } = CatArea.Quarantine;
    public Gender Gender { get; private set; } = Gender.Unknown;
    public FoodType FoodType { get; private set; } = FoodType.FirstPhase;
    public LitterGranuleType LitterGranuleType { get; private set; } = LitterGranuleType.Normal;
    public int CageNumber { get; private set; } = 1;

    public void Store(AgeCategory value) { AgeCategory = value; }
    public void Store(CatBehaviour value) { CatBehaviour = value; }
    public void Store(CatchOriginType value) { CatchOriginType = value; }
    public void Store(CatArea value) { CatArea = value; }
    public void Store(Gender value) { Gender = value; }
    public void Store(FoodType value) { FoodType = value; }
    public void Store(LitterGranuleType value) { LitterGranuleType = value; }
    public void Store(int value) { CageNumber = value; }
}
