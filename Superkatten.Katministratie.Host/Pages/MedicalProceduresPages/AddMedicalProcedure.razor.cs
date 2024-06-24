using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Superkatten.Katministratie.Contract.ApiInterface;
using Superkatten.Katministratie.Contract.Entities;
using Superkatten.Katministratie.Contract.Language;
using Superkatten.Katministratie.Host.Helpers;
using Superkatten.Katministratie.Host.Services;
using Superkatten.Katministratie.Host.Services.Interfaces;

namespace Superkatten.Katministratie.Host.Pages.MedicalProceduresPages;

partial class AddMedicalProcedure
{
    [Inject] IStringLocalizer<KatministratieApp> Localizer { get; set; } = null!;
    [Inject] private IMedicalProcedureService _medicalprocedureService { get; set; } = null!;
    [Inject] private Navigation _navigation { get; set; } = null!;
    [Inject] private ISuperkattenListService _superkattenService { get; set; } = null!;

    
    [Parameter] public Guid SuperkatId { get; set; }

    private Superkat? _superkat;

    private DateTime TimeStamp { get; set; } = DateTime.UtcNow;
    private string Remark { get; set; } = string.Empty;

    private MedicalProcedureType _selectedMedicalProcedureType;
    private static List<MedicalProcedureType> _medicalProcedures = null!;
    private static List<string> _medialProcedureNames = null!;

    protected override async Task OnInitializedAsync()
    {
        var superkat = await _superkattenService.GetSuperkatAsync(SuperkatId);
        _superkat = superkat ?? throw new ArgumentNullException(nameof(superkat));

        await BuildMedialProcedureSelectionList();

        _medialProcedureNames = _medicalProcedures
            .Select(x => Localizer[x.GetType().Name + x.ToString()].Value)
            .ToList();
    }

    private async Task BuildMedialProcedureSelectionList()
    {
        _medicalProcedures = Enum.GetValues(typeof(MedicalProcedureType)).Cast<MedicalProcedureType>().ToList();

        if (_superkat is null)
        {
            return;
        }

        var allMedicalProcedures = await _medicalprocedureService.GetAllMedicalProcedures();
        var isNeutralized = allMedicalProcedures
            .Where(s => s.Id == _superkat.Id && s.ProcedureType == MedicalProcedureType.Neutralize)
            .Any();

        if (isNeutralized)
        {
            _medicalProcedures.Remove(MedicalProcedureType.Neutralize);
        }
    }

    private Task OnSelectMedialProcedure(MedicalProcedureType medialProcedure)
    {
        _selectedMedicalProcedureType = medialProcedure;
        StateHasChanged();

        return Task.CompletedTask;
    }

    private async Task OnOk()
    {
        if (_superkat is null)
        {
            return;
        }

        var parameters = new AddMedicalProcedureParameters
        {
            SuperkatId = _superkat.Id,
            Remark = Remark,
            ProcedureType = _selectedMedicalProcedureType,
            Timestamp = TimeStamp
        };

        await _medicalprocedureService.AddMedicalProcedureAsync(_superkat.Id, parameters);

        _navigation.NavigateBack();
    }

    private void OnCancel()
    {
        _navigation.NavigateBack();
    }
}
