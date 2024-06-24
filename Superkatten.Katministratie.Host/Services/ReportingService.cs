using Superkatten.Katministratie.Contract.ApiInterface.Reporting;
using Superkatten.Katministratie.Host.Services.Http;
using Superkatten.Katministratie.Host.Services.Interfaces;

namespace Superkatten.Katministratie.Host.Services;

public class ReportingService : IReportingService
{
    private readonly IHttpService _httpService;

    public ReportingService(IHttpService httpService)
    {
        _httpService = httpService;
    }

    public async Task EmailInventoryDetailsReportAsync(RequestCatchOriginEmailParameters requestPeriod)
    {
        var uri = "api/Reporting/reports/catchOrigin";
        await _httpService.Put(uri, requestPeriod);
    }

    public async Task EmailCageCardAsync(RequestCageCardEmailParameters cageCardRequestParameters)
    {
        var uri = "api/Reporting/reports/cagecard";
        await _httpService.Put(uri, cageCardRequestParameters);
    }

    public async Task EmailNotNeutralizedAdopteesReportAsync(string email)
    {
        var uri = "api/Reporting/reports/notNeutralizedAdoptees";
        await _httpService.Put(uri, email);
    }

    public async Task EmailNotNeutralizedInRefugeReportAsync(string email)
    {
        var uri = "api/Reporting/reports/notNeutralizedRefuge";
        await _httpService.Put(uri, email);
    }
}
