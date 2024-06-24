using Superkatten.Katministratie.Contract.ApiInterface.Reporting;

namespace Superkatten.Katministratie.Host.Services.Interfaces;

public interface IReportingService
{
    Task EmailInventoryDetailsReportAsync(RequestCatchOriginEmailParameters requestPeriod);
    Task EmailCageCardAsync(RequestCageCardEmailParameters cageCardRequestParameters);
    Task EmailNotNeutralizedAdopteesReportAsync(string email);
    Task EmailNotNeutralizedInRefugeReportAsync(string email);
}
