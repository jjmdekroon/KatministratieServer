using Superkatten.Katministratie.Application.CageCard.Details;
using Superkatten.Katministratie.Domain.Entities;
using System.Collections.Generic;
using System.ComponentModel;

using QuestPDFInfrastructure = QuestPDF.Infrastructure;

namespace Superkatten.Katministratie.Application.CageCard;

public class CageCardComposerFactory : ICageCardComposerFactory
{
    public QuestPDFInfrastructure.IComponent GetContentComposer(CageCardType cageCardType, IReadOnlyCollection<Superkat> superkatten)
    {
        return cageCardType switch
        {
            CageCardType.Default => new CageCardDefaultContentComposer(superkatten),
            CageCardType.List => new SuperkattenListContentComponent(superkatten),
            _ => throw new InvalidEnumArgumentException(nameof(cageCardType), (int)cageCardType, typeof(CageCardType))
        };
    }

    public QuestPDFInfrastructure.IComponent GetFooterComposer(CageCardType cageCardType, IReadOnlyCollection<Superkat> superkatten)
    {
        return cageCardType switch
        {
            CageCardType.Default => new CageCardDefaultFooterComposer(superkatten),
            CageCardType.List => new CageCardDefaultFooterComposer(superkatten),
            _ => throw new InvalidEnumArgumentException(nameof(cageCardType), (int)cageCardType, typeof(CageCardType))
        };
    }

    public QuestPDFInfrastructure.IComponent GetHeaderComposer(CageCardType cageCardType, IReadOnlyCollection<Superkat> superkatten)
    {
        return cageCardType switch
        {
            CageCardType.Default => new CageCardDefaultHeaderComposer(superkatten),
            CageCardType.List => new CageCardDefaultHeaderComposer(superkatten),
            _ => throw new InvalidEnumArgumentException(nameof(cageCardType), (int)cageCardType, typeof(CageCardType))
        };
    }
}
