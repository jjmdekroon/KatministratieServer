using Microsoft.AspNetCore.Mvc;
using Superkatten.Katministratie.Application.Authorization;
using Superkatten.Katministratie.Application.Interfaces;
using Superkatten.Katministratie.Application.Mappers;
using Superkatten.Katministratie.Contract.ApiInterface;
using Superkatten.Katministratie.Domain.Entities;

namespace Superkatten.Katministratie.SuperkatApi.Controllers
{
    [AuthorizeRoles(PermissionEnum.Administrator, PermissionEnum.Coordinator)]
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly ILocationService _service;
        private readonly ILocationMapper _locationMapper;

        public LocationController(ILocationService service, ILocationMapper mapper)
        {
            _service = service;
            _locationMapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllGastgezinnen()
        {
            var gastgezinnen = await _service.GetLocationsAsync();

            return Ok(gastgezinnen
                .Select(_locationMapper.ToContract)
                .ToList()
            );
        }

        [HttpGet]
        [Route("adopter")]
        [AuthorizeRoles(PermissionEnum.Adopter)]
        public async Task<IActionResult> GetAdoptant(Guid Id)
        {
            var locations = await _service.GetLocationsAsync();

            return Ok(locations
                .Where(o => o.Id == Id)
                .Select(_locationMapper.ToContract)
                .ToList()
            );
        }

        [HttpPut]
        public async Task<IActionResult> PutGastgezin([FromBody] LocationNawParameters parameters)
        {
            var gastgezin = await _service.CreateHostFamilyAsync(parameters);
            
            return Ok(
                _locationMapper.ToContract(gastgezin)
            );
        }

        [HttpPost]
        public async Task<IActionResult> PostGastgezin(Guid id, [FromBody] LocationNawParameters updateGastgezinParameters)
        {
            var gastgezin = await _service.UpdateLocationAsync(id, updateGastgezinParameters);

            return Ok(
                _locationMapper.ToContract(gastgezin)
            );
        }
        
        [HttpDelete]
        public async Task<IActionResult> DeleteGastgezin(Guid id)
        {
            await _service.DeleteLocationAsync(id);

            return Ok();
        }
    }
}
