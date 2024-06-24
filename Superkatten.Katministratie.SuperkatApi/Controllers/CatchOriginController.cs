using Microsoft.AspNetCore.Mvc;
using Superkatten.Katministratie.Application.Authorization;
using Superkatten.Katministratie.Application.Interfaces;
using Superkatten.Katministratie.Application.Mappers;
using Superkatten.Katministratie.Domain.Entities;

namespace Superkatten.Katministratie.SuperkatApi.Controllers;

[AuthorizeRoles(PermissionEnum.Administrator, PermissionEnum.Coordinator)]
[ApiController]
[Route("api/[controller]")]
public class CatchOriginController : ControllerBase
{
    private readonly ICatchOriginService _catchOriginService;

    public CatchOriginController(ICatchOriginService catchOriginService)
    {
        _catchOriginService = catchOriginService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var catchOrigin = await _catchOriginService.GetCatchOriginsAsync();
        return Ok(catchOrigin);
    }
}
