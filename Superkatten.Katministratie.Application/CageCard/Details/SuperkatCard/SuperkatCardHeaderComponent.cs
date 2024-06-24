using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Superkatten.Katministratie.Domain.Entities;

namespace Superkatten.Katministratie.Application.CageCard.Details.SuperkatCard;

internal class SuperkatCardHeaderComponent : IComponent
{
    private readonly Superkat _superkat;

    public SuperkatCardHeaderComponent(Superkat superkat)
    {
        _superkat = superkat;
    }

    public void Compose(IContainer container)
    {
        container.Row(row =>
        {
            row.RelativeItem()
                .PaddingLeft(2)
                .AlignLeft()
                .Text(_superkat.UniqueNumber)
                .FontColor(Colors.White);

            row.RelativeItem()
                .PaddingRight(2)
                .AlignRight()
                .Text("TNRC")
                .FontColor(Colors.White);
        });
    }
}
