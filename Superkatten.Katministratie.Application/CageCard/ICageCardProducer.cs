using Superkatten.Katministratie.Domain.Entities;
using System.Collections.Generic;

namespace Superkatten.Katministratie.Application.CageCard;

public interface ICageCardProducer
{
    byte[]? CreateCageCard(IReadOnlyCollection<Superkat> superkatten);

    byte[]? CreateSuperkattenReport(IReadOnlyCollection<Superkat> superkatten);
}
