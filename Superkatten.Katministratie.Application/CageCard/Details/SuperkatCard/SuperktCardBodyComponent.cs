using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using Superkatten.Katministratie.Domain.Entities;

namespace Superkatten.Katministratie.Application.CageCard.Details.SuperkatCard;

internal class SuperktCardBodyComponent : IComponent
{
    private readonly Superkat _superkat;

    public SuperktCardBodyComponent(Superkat superkat)
    {
        _superkat = superkat;
    }

    public void Compose(IContainer container)
    {
        var superkatPhoto = new SuperkatPhotoOrNameComponent(_superkat);
        var superkatDetails = new SuperkatDetailsComponent(_superkat);
        container.Row(row =>
        {
            row.RelativeItem()
                .Padding(2)
                .Element(superkatPhoto
                .Compose);

            row.RelativeItem()
                .PaddingLeft(2)
                .PaddingRight(2)
                .Element(superkatDetails.Compose);
        });
    }
}
