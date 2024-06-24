using QuestPDF.Infrastructure;
using Superkatten.Katministratie.Domain.Entities;

namespace Superkatten.Katministratie.Application.CageCard.Details.SuperkatCard;

public class SuperkatRowComponent : IComponent
{
    private readonly Superkat _superkat;

    public SuperkatRowComponent(Superkat superkat)
    {
        _superkat = superkat;
    }

    public void Compose(IContainer container)
    {
        throw new System.NotImplementedException();
    }
}
