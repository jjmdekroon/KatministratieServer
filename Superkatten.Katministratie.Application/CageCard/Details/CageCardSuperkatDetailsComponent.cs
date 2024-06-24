using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Superkatten.Katministratie.Application.CageCard.Details.SuperkatCard;
using Superkatten.Katministratie.Domain.Entities;
using System;

namespace Superkatten.Katministratie.Application.CageCard.Details;

public class CageCardSuperkatDetailsComponent : IComponent
{
    private readonly Superkat _superkat;

    public CageCardSuperkatDetailsComponent(Superkat superkat)
    {
        _superkat = superkat;
    }

    public void Compose(IContainer container)
    {
        var cardHeader = new SuperkatCardHeaderComponent(_superkat);
        var cardBody = new SuperktCardBodyComponent(_superkat);
        container.Column(column =>
            {
                column.Item()
                    .Background(Colors.Black)
                    .Element(cardHeader.Compose);

                column.Item()
                    .PaddingLeft(15)
                    .Background(Colors.White)
                    .Element(cardBody.Compose);
            });
    }
}
