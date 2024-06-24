using Superkatten.Katministratie.Contract.ApiInterface;
using Superkatten.Katministratie.Host.Services.Http;
using Superkatten.Katministratie.Host.Services.Interfaces;

namespace Superkatten.Katministratie.Host.Services;

public class MedicalProcedureService : IMedicalProcedureService
{
    private readonly IHttpService _httpService;

    public MedicalProcedureService(IHttpService httpService)
    {
        _httpService = httpService;
    }

    public Task AddMedicalProcedureAsync(Guid superkatId, AddMedicalProcedureParameters parameters)
    {
        var uri = $"api/MedicalProcedure/?superkatId={superkatId}";
        return _httpService.Put(uri, parameters);
    }

    public async Task<IReadOnlyCollection<MedicalProcedureInformation>> GetAllMedicalProcedures()
    {
        var uri = "api/MedicalProcedure";

            var allMedicalProcedures = await _httpService.Get<List<MedicalProcedureInformation>>(uri);

        return allMedicalProcedures is null
            ? new List<MedicalProcedureInformation>()
            : allMedicalProcedures;
    }
}
