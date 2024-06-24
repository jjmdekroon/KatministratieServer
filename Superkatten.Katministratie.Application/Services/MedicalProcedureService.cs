using Microsoft.Extensions.Logging;
using Superkatten.Katministratie.Application.Exceptions;
using Superkatten.Katministratie.Application.Interfaces;
using Superkatten.Katministratie.Application.Mappers;
using Superkatten.Katministratie.Contract.ApiInterface;
using Superkatten.Katministratie.Contract.Entities;
using Superkatten.Katministratie.Infrastructure.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Superkatten.Katministratie.Application.Services;

public class MedicalProcedureService : IMedicalProcedureService
{
    private readonly ILogger<MedicalProcedureService> _logger;
    private readonly ISuperkattenRepository _superkattenRepository;
    private readonly IMedicalProceduresRepository _medicalProceduresRepository;
    private readonly IMedicalProcedureMapper _medialProcedureMapper;

    public MedicalProcedureService(
        ILogger<MedicalProcedureService> logger,
        ISuperkattenRepository superkattenRepository,
        IMedicalProceduresRepository medicalProceduresRepository,
        IMedicalProcedureMapper medialProcedureMapper
    )
    {
        _logger = logger;
        _superkattenRepository = superkattenRepository;
        _medicalProceduresRepository = medicalProceduresRepository;
        _medialProcedureMapper = medialProcedureMapper;
    }

    public async Task AddMedicalProcedureAsync(AddMedicalProcedureParameters parameters)
    {
        var superkat = await _superkattenRepository.GetSuperkatAsync(parameters.SuperkatId);
        if (superkat is null)
        {
            throw new ServiceException($"Superkat with id '{parameters.SuperkatId}' does not exist");
        }

        var medicalProcedure = _medialProcedureMapper.MapToDomain(parameters);

        await _medicalProceduresRepository.AddMedicalProcedureAsync(medicalProcedure);
    }

    public async Task<IReadOnlyCollection<MedicalProcedureInformation>> GetAllMedicalProceduresAsync()
    {
        var medicalProcedures = await _medicalProceduresRepository.GetAllMedicalProcedureAsync();

        var result = new List<MedicalProcedureInformation>();
        foreach(var medicalProcedure in medicalProcedures)
        {
            var superkat = await _superkattenRepository.GetSuperkatAsync(medicalProcedure.SuperkatId);
            if (superkat is null)
            {
                throw new ServiceException($"Superkat with Id '{medicalProcedure.SuperkatId}' cannot be found");
            }

            var medicalProcedureInformation = _medialProcedureMapper.MapToContract(superkat, medicalProcedure);
            result.Add(medicalProcedureInformation);
        }

        return result.OrderBy(o=>o.UniqueNumber).ToList();
    }
}
