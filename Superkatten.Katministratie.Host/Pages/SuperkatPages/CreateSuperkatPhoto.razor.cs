using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Superkatten.Katministratie.Contract.ApiInterface;
using Superkatten.Katministratie.Host.Helpers;
using Superkatten.Katministratie.Host.Services;
using Superkatten.Katministratie.Contract.Entities;
using Superkatten.Katministratie.Host.Entities;

namespace Superkatten.Katministratie.Host.Pages.SuperkatPages;

// Javascript from https://wellsb.com/csharp/aspnet/blazor-webcam-capture
// see also https://developer.mozilla.org/en-US/docs/Web/API/MediaDevices/getUserMedia

public partial class CreateSuperkatPhoto
{
    [Inject] private IJSRuntime JSRuntime { get; set; } = null!;

    [Inject] public Navigation Navigation { get; set; } = null!;

    [Inject] public ISuperkattenListService SuperkattenService { get; set; } = null!;

    [Parameter]
    public Guid? SuperkatId { get; set; }

    private IReadOnlyCollection<Superkat> _superkatten = Array.Empty<Superkat>();
    private IReadOnlyCollection<string> _superkatNames = Array.Empty<string>();
    private IReadOnlyCollection<MediaDeviceInfoModel> _cameraDevices = new List<MediaDeviceInfoModel>();
    private IReadOnlyCollection<string> _cameraDeviceNames = new List<string>();
    private Superkat? InitialSuperkat { get; set; }
    private Superkat? _selectedSuperkat;

    private bool IsInitializing { get; set; } = true;

    private bool SuperkatIsSelected => _selectedSuperkat is null;

    protected async override Task OnInitializedAsync()
    {
        // See https://developer.mozilla.org/en-US/docs/Web/API/MediaDevices/getUserMedia
        var cameraDevices = await JSRuntime.InvokeAsync<MediaDeviceInfoModel[]>("getCameraDeviceList");
        var cameraDeviceNames = cameraDevices.Select(c => c.Label).ToList();

        var superkatten = await SuperkattenService.GetAllSuperkattenAsync();

        _superkatten = superkatten.OrderBy(s => s.UniqueNumber).ToList();
        _superkatNames = _superkatten.Select(s => s.UniqueNumber).ToList();

        _cameraDevices = cameraDevices.ToList();
        _cameraDeviceNames = cameraDeviceNames;

        await OnSelectCameraDeviceAsync(_cameraDevices.Last());

        if (SuperkatId is not null)
        {
            InitialSuperkat = superkatten
                .Where(s => s.Id == SuperkatId)
                .FirstOrDefault();

            _selectedSuperkat = InitialSuperkat;
        }

        IsInitializing = false;
    }

    private async Task OnOk()
    {
        await JSRuntime.InvokeAsync<string>(
            "getFrame", 
            "videoFeed", 
            "currentFrame", 
            DotNetObjectReference.Create(this)
        );
    }

    private void OnCancel()
    {
        Navigation.NavigateBack();
    }

    private void OnSelectSuperkat(Superkat superkat)
    {
        _selectedSuperkat = superkat;
    }

    private async Task OnSelectCameraDeviceAsync(MediaDeviceInfoModel selectedCameraDevice)
    {
        await JSRuntime.InvokeVoidAsync("startVideo", "videoFeed", selectedCameraDevice.DeviceId);
    }

    [JSInvokable]
    public async Task ProcessImage(string imageString)
    {
        if (_selectedSuperkat is null)
        {
            return;
        }

        var updateSuperkatPhotoParameters = new PhotoParameters
        {
            Photo = Convert.FromBase64String(imageString.Split(',')[1])
        };

        await SuperkattenService.UpdateSuperkatPhoto(_selectedSuperkat.Id, updateSuperkatPhotoParameters);

        await JSRuntime.InvokeVoidAsync("stopVideo");

        Navigation.NavigateBack();
    }
}