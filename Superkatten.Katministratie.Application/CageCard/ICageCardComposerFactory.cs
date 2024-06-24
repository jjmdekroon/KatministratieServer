using Superkatten.Katministratie.Domain.Entities;
using System.Collections.Generic;

using QuestPDFInfrastructure = QuestPDF.Infrastructure;

namespace Superkatten.Katministratie.Application.CageCard;

public interface ICageCardComposerFactory
{
    QuestPDFInfrastructure.IComponent GetHeaderComposer(CageCardType cageCardType, IReadOnlyCollection<Superkat> superkatten);
    QuestPDFInfrastructure.IComponent GetContentComposer(CageCardType cageCardType, IReadOnlyCollection<Superkat> superkatten);
    QuestPDFInfrastructure.IComponent GetFooterComposer(CageCardType cageCardType, IReadOnlyCollection<Superkat> superkatten);
}
