using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using Superkatten.Katministratie.Domain.Entities;

namespace Superkatten.Katministratie.Application.CageCard.Details.SuperkatCard;

public class SuperkatDetailsComponent : IComponent
{
    private readonly Superkat _superkat;

    public SuperkatDetailsComponent(Superkat superkat)
    {
        _superkat = superkat;
    }

    public void Compose(IContainer container)
    {
        container.Column(column =>
        {
            column.Item().Element(new SuperkatDetailComponent("Gevangen op:", _superkat.CatchDate.ToShortDateString()).Compose);
            column.Item().Element(new SuperkatDetailComponent("Geboren op:", _superkat.Birthday.ToShortDateString()).Compose);
            column.Item().Element(new SuperkatDetailComponent("Kleur:", _superkat.Color.ToString()).Compose);
            column.Item().Element(new SuperkatDetailComponent("Behaviour:", _superkat.Behaviour.ToString()).Compose);
        });
    }
}
