using Microsoft.AspNetCore.Mvc;
using Superkatten.Katministratie.Application.Authorization;
using Superkatten.Katministratie.Application.Interfaces;
using Superkatten.Katministratie.Domain.Entities;

namespace Superkatten.Katministratie.SuperkatApi.Controllers
{
    [AuthorizeRoles(PermissionEnum.Administrator, PermissionEnum.Coordinator)]
    [Route("api/[Controller]")]
    [ApiController]
    public class SuperkatActionController : ControllerBase
    {
        private readonly ISuperkatAction _actionService;

        public SuperkatActionController(ISuperkatAction service)
        {
            _actionService = service;
        }

        [HttpPut]
        [Route("ToggleReserve")]
        public async Task<IActionResult> ToggleReserve([FromBody] Guid id)
        {
            await _actionService.ToggleReserveAsync(id);

            return Ok();
        }

        [HttpPut]
        [Route("ToggleRetour")]
        public async Task<IActionResult> ToggleRetour([FromBody] Guid id)
        {
            await _actionService.ToggleRetourAsync(id);

            return Ok();
        }
    }
}
