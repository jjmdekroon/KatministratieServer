using Microsoft.AspNetCore.Mvc;
using Superkatten.Katministratie.Application.Authorization;
using Superkatten.Katministratie.Application.Interfaces;
using Superkatten.Katministratie.Domain.Entities;

using ContractEntities = Superkatten.Katministratie.Contract.Entities;

namespace Superkatten.Katministratie.SuperkatApi.Controllers;

[AuthorizeRoles(PermissionEnum.Administrator, PermissionEnum.Coordinator)]
[Route("api/[Controller]")]
[ApiController]
public class SettingsController : ControllerBase
{
    private readonly ISettingsService _settingsService;

    public SettingsController(ISettingsService settingsService)
    {
        _settingsService = settingsService;
    }

    [HttpPut]
    [Route("CageNumbersForCatArea")]
    public IActionResult GetCageNumbersForCageArea([FromBody]ContractEntities.CatArea cageArea)
    {
        var cageNumbers = _settingsService.GetCageNumbersForCageAreaAsync(cageArea);

        return Ok(cageNumbers);
    }
}
