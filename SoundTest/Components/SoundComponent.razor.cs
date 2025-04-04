namespace SoundTest.Components;

public partial class SoundComponent(IJSRuntime jsRuntime, ISnackbar snackbar, NavigationManager navigation)
{
    private bool isPlaying = false;
    private string? soundLink;

    private Types type;
    private int frequency;

    private IJSObjectReference? module;
    private bool isJsInitialized = false;

    private List<AudioDevice>? AudioDevices { get; set; }

    private string? SelectedDeviceId { get; set; }

    [Parameter]
#pragma warning disable BL0007 // Component parameters should be auto properties
    public Types Type
#pragma warning restore BL0007 // Component parameters should be auto properties
    {
        get => type;
        set
        {
            type = value;
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            SetParametersAndUpdate();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }
    }

    [Parameter]
#pragma warning disable BL0007 // Component parameters should be auto properties
    public int Frequency
#pragma warning restore BL0007 // Component parameters should be auto properties
    {
        get => frequency;
        set
        {
            frequency = value switch
            {
                < MinFrequency => MinFrequency,
                > MaxFrequency => MaxFrequency,
                _ => value,
            };
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            SetParametersAndUpdate();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }
    }

    private async Task SetParametersAndUpdate()
    {
        await SetParameters();
        UpdateUri();
    }

    private void UpdateUri()
    {
        var uri = navigation.GetUriWithQueryParameters(new Dictionary<string, object?>()
        {
            [nameof(type)] = (int)type, [nameof(frequency)] = frequency,
        });
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
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            SetAudioDevice(SelectedDeviceId);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }
    }
}
