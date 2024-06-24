using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Superkatten.Katministratie.Domain.Entities;
using System.Collections.Generic;

namespace Superkatten.Katministratie.Application.CageCard;

public class CageCardDocument : IDocument
{
    // See: https://www.questpdf.com/quick-start.html

    private IReadOnlyCollection<Superkat> _superkatten = new List<Superkat>();
    private readonly ICageCardComposerFactory _composerFactory;

    public CageCardDocument(ICageCardComposerFactory composerFactory, IReadOnlyCollection<Superkat> superkatten)
    {
        _superkatten = superkatten;
        _composerFactory = composerFactory;
    }

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

    public void Compose(IDocumentContainer container)
    {
        var headerComponent = _composerFactory.GetHeaderComposer(CageCardType.Default, _superkatten);
        var contentComponent = _composerFactory.GetContentComposer(CageCardType.Default, _superkatten);
        var footerComponent = _composerFactory.GetFooterComposer(CageCardType.Default, _superkatten);

        container.Page(page =>
        {
            page.Margin(50);

            page.Header().Background(Colors.White).Element(headerComponent.Compose);
            page.Content().Background(Colors.White).Element(contentComponent.Compose);
            page.Footer().Background(Colors.White).Element(footerComponent.Compose);
        });
    }
}
