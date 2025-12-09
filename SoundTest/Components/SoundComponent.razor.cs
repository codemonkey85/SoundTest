namespace SoundTest.Components;

public partial class SoundComponent(IJSRuntime jsRuntime, ISnackbar snackbar, NavigationManager navigation)
{
    private bool isJsInitialized;
    private bool isPlaying;

    private IJSObjectReference? module;
    private string? soundLink;

    private List<AudioDevice>? AudioDevices { get; set; }

    private string? SelectedDeviceId { get; set; }

    [Parameter]
    public Types Type
    {
        get;
        set
        {
            field = value;
            _ = SetParametersAndUpdate();
        }
    }

    [Parameter]
    public int Frequency
    {
        get;
        set
        {
            field = value switch
            {
                < MinFrequency => MinFrequency,
                > MaxFrequency => MaxFrequency,
                _ => value
            };
            _ = SetParametersAndUpdate();
        }
    }

    private async Task SetParametersAndUpdate()
    {
        await SetParameters();
        UpdateUri();
    }

    private void UpdateUri()
    {
        var uri = navigation.GetUriWithQueryParameters(new Dictionary<string, object?> { [nameof(Type)] = (int)Type, [nameof(Frequency)] = Frequency });
        soundLink = uri;
    }

    protected override async Task OnInitializedAsync() => await InitializeJs();

    private async Task InitializeJs()
    {
        if (OperatingSystem.IsBrowser() && !isJsInitialized)
        {
            try
            {
                await JSHost.ImportAsync("soundtest.js",
                    $"../{nameof(Components)}/{nameof(SoundComponent)}.razor.js");

                module = await jsRuntime.InvokeAsync<IJSObjectReference>(
                    "import", $"../{nameof(Components)}/{nameof(SoundComponent)}.razor.js");

                isJsInitialized = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }

    private async Task SetParameters()
    {
        if (!isJsInitialized)
        {
            await InitializeJs();
        }

        JsInterop.SetParameters(Type.ToString().ToLower(), Frequency);
    }

    private void StartPlaying()
    {
        JsInterop.StartPlaying();
        isPlaying = true;
    }

    private void StopPlaying()
    {
        JsInterop.StopPlaying();
        isPlaying = false;
    }

    private void CopySoundLink()
    {
        if (soundLink == null)
        {
            return;
        }

        JsInterop.CopyTextToClipboard(soundLink);
        snackbar.Configuration.PositionClass = Defaults.Classes.Position.BottomLeft;
        snackbar.Add("Sound link copied to clipboard");
    }

    private async Task SetComfortableTone()
    {
        Type = Types.Sine;
        Frequency = ComfortableFrequency;
        await SetParameters();
    }

    private async Task SetNeuroTone()
    {
        Type = Types.Sine;
        Frequency = NeuroFrequency;
        await SetParameters();
    }

    private async Task SetPerfectCTone()
    {
        Type = Types.Sine;
        Frequency = (int)PerfectCFrequency;
        await SetParameters();
    }

    private async Task SetAudioDevice(string deviceId)
    {
        if (module is null)
        {
            return;
        }

        await module.SetAudioDevice(deviceId);
    }

    private void AudioDeviceChanged()
    {
        if (SelectedDeviceId is not null)
        {
            _ = SetAudioDevice(SelectedDeviceId);
        }
    }
}
