using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Superkatten.Katministratie.Application.CageCard.Details.SuperkatCard;

public class SuperkatDetailComponent : IComponent
{
    private readonly string _label;
    private readonly string _text;

    public SuperkatDetailComponent(string label, string text)
    {
        _label = label;
        _text = text;
    }

    public void Compose(IContainer container)
    {
        container.Column(column =>
        {
            column.Item()
                .Padding(0)
                .BorderBottom(1)
                .BorderColor(Colors.Grey.Lighten1)
                .AlignLeft()
                .Text(_label)
                .FontSize(6)
                .FontColor(Colors.Grey.Lighten1);

            column.Item()
                .Padding(0)
                .PaddingLeft(1)
                .AlignLeft()
                .Text(_text)
                .FontSize(10);
        });
    }
}
