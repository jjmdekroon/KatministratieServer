using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Superkatten.Katministratie.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Superkatten.Katministratie.Application.CageCard;

public class SuperkattenListDocument : IDocument
{
    private readonly IReadOnlyCollection<Superkat> _superkatten = new List<Superkat>();
    private readonly ICageCardComposerFactory _composerFactory;

    public SuperkattenListDocument(ICageCardComposerFactory composerFactory, IReadOnlyCollection<Superkat> superkatten)
    {
        _superkatten = superkatten;
        _composerFactory = composerFactory;
    }

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

    public void Compose(IDocumentContainer container)
    {
        var headerComponent = _composerFactory.GetHeaderComposer(CageCardType.Default, _superkatten);
        var contentComponent = _composerFactory.GetContentComposer(CageCardType.List, _superkatten);
        var footerComponent = _composerFactory.GetFooterComposer(CageCardType.Default, _superkatten);

        container.Page(page =>
        {
            page.Margin(50);

            page.Header().Background(Colors.Grey.Lighten5).Element(headerComponent.Compose);
            page.Content().Background(Colors.White).Element(contentComponent.Compose);
            page.Footer().Background(Colors.Grey.Lighten5).Element(footerComponent.Compose);
        });
    }
}
