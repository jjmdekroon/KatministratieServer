using Microsoft.AspNetCore.Mvc;
using Superkatten.Katministratie.Application.Authorization;
using Superkatten.Katministratie.Application.Interfaces;
using Superkatten.Katministratie.Contract.ApiInterface;
using Superkatten.Katministratie.Domain.Entities;

namespace Superkatten.Katministratie.SuperkatApi.Controllers;

[AuthorizeRoles(PermissionEnum.Administrator, PermissionEnum.Coordinator, PermissionEnum.Gastgezin)]
[Route("api/[Controller]")]
[ApiController]
public class MedicalProcedureController : ControllerBase
{
    private readonly IMedicalProcedureService _medicalProcedureService;

    public MedicalProcedureController(IMedicalProcedureService medicalProcedureService)
    {
        _medicalProcedureService = medicalProcedureService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var medicalProcedures = await _medicalProcedureService.GetAllMedicalProceduresAsync();

        return Ok(medicalProcedures);
    }

    [HttpPut]
    public async Task<IActionResult> AddMedicalProcedure(AddMedicalProcedureParameters medicalProcedureParameters)
    {
        await _medicalProcedureService.AddMedicalProcedureAsync(medicalProcedureParameters);

        return Ok();
    }
}
