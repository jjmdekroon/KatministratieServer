using Microsoft.AspNetCore.Mvc;
using Superkatten.Katministratie.Application.Authorization;
using Superkatten.Katministratie.Application.Interfaces;
using Superkatten.Katministratie.Contract.ApiInterface.Reporting;
using Superkatten.Katministratie.Domain.Entities;
using Superkatten.Katministratie.Infrastructure.Interfaces;

namespace Superkatten.Katministratie.SuperkatApi.Controllers;

[AuthorizeRoles(PermissionEnum.Administrator, PermissionEnum.Coordinator)]
[Route("api/[Controller]")]
[ApiController]
public class ReportingController : ControllerBase
{
    private readonly IReportingService _reportingService;

    public ReportingController(IReportingService reportingService)
    {
        _reportingService = reportingService;
    }

    [HttpPut]
    [Route("reports/catchOrigin")]
    public async Task<IActionResult> EmailCatchOriginReport([FromBody] RequestCatchOriginEmailParameters requestCatchOriginParameters)
    {
        await _reportingService.EmailCatchOriginReport(
            requestCatchOriginParameters.Email,
            requestCatchOriginParameters.From,
            requestCatchOriginParameters.To
        );

        return Ok();
    }

    [HttpPut]
    [Route("reports/cagecard")]
    public async Task<IActionResult> EmailCageCard([FromBody] RequestCageCardEmailParameters requestCageCardParameters)
    {
        await _reportingService.EmailCageCard(
            requestCageCardParameters.Email,
            requestCageCardParameters.CatArea,
            requestCageCardParameters.CageNumber
        );

        return Ok();
    }

    [HttpPut]
    [Route("reports/cagecard/notNeutralizedAdoptees")]
    public async Task<IActionResult> EmailNotNeutralizedAdoptees([FromBody] string email)
    {
        await _reportingService.EmailNotNeutralizedAdopteesReport(email);
        return Ok();
    }

    [HttpPut]
    [Route("reports/cagecard/notNeutralizedRefuge")]
    public async Task<IActionResult> EmailNotNeutralizedRefuge([FromBody] string email)
    {
        await _reportingService.EmailNotNeutralizedRefugeReport(email);
        return Ok();
    }
}
