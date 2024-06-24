using Superkatten.Katministratie.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Superkatten.Katministratie.Infrastructure.Interfaces;

public interface IReportingRepository
{
    Task<IReadOnlyCollection<Superkat>> GetSuperkattenBetweenFromToAsync(DateTime from, DateTime to);
    Task<IReadOnlyCollection<Superkat>> GetSuperkattenAtLocationAsync(CatArea catArea, int? cageNumber);
    Task<IReadOnlyCollection<Superkat>> GetNotNeutralizedSuperkatten();
}
