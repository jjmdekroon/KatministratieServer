using Microsoft.AspNetCore.Components;
using Superkatten.Katministratie.Host.Helpers;
using Superkatten.Katministratie.Host.Services.Interfaces;
using Superkatten.Katministratie.Contract.ApiInterface;
using Superkatten.Katministratie.Contract.Entities;
using Microsoft.Extensions.Localization;
using Superkatten.Katministratie.Contract.Language;

namespace Superkatten.Katministratie.Host.Pages.MedicalProceduresPages;

public partial class OverviewMedicalProcedures
{
    [Inject] IStringLocalizer<KatministratieApp> Localizer { get; set; } = null!;
    [Inject] private Navigation _navigation { get; set; } = null!;
    [Inject] private IMedicalProcedureService _medicalProcedureService { get; set; } = null!;


    private Dictionary<string, List<MedicalProcedureInformation>> MedicalProcedureInformationDictionary { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        var medicalProcedures = await _medicalProcedureService.GetAllMedicalProcedures();

        medicalProcedures = medicalProcedures
            .OrderByDescending(o => o.Timestamp)
            .ToList();

        foreach (var medicalProcedure in medicalProcedures)
        {
            var key = MedicalProcedureInformationDictionary.TryAdd(
                medicalProcedure.UniqueNumber ?? "unkown", 
                new List<MedicalProcedureInformation>()
            );

            MedicalProcedureInformationDictionary[medicalProcedure.UniqueNumber ?? "unkown"].Add(medicalProcedure);
        }
    }

    private void OnBack()
    {
        if (_navigation is null)
        {
            return;
        }

        _navigation.NavigateBack();
    }

    private string LocalizeMedicalProcedure(MedicalProcedureType procedureType)
    {
        var key = procedureType.GetType().Name + procedureType.ToString();
        return Localizer[key].Value;
    }
}


