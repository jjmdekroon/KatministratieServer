using Superkatten.Katministratie.Domain.Entities;
using System.Collections.Generic;

namespace Superkatten.Katministratie.Application.Reporting;

public interface IReportBuilder
{
    string BuildSuperkattenInventory(IReadOnlyCollection<Superkat> superkatten);
}
