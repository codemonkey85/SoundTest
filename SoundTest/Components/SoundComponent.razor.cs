#pragma warning disable BL0007
#pragma warning disable CS4014
#pragma warning disable IDE0058

using Microsoft.JSInterop;

namespace SoundTest.Components;

public partial class SoundComponent
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
    public Types Type
    {
        get => type;
        set
        {
            type = value;
            SetParametersAndUpdateAsync();
        }
    }

    [Parameter]
    public int Frequency
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
            SetParametersAndUpdateAsync();
        }
    }

    private async Task SetParametersAndUpdateAsync()
    {
        await SetParametersAsync();
        UpdateUri();
    }

    private void UpdateUri()
    {
        var uri = Navigation.GetUriWithQueryParameters(new Dictionary<string, object?>()
        {
            [nameof(type)] = (int)type,
            [nameof(frequency)] = frequency,
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
                module = await JsRuntime.InvokeAsync<IJSObjectReference>(
                    "import", $"../{nameof(Components)}/{nameof(SoundComponent)}.razor.js");
                isJsInitialized = true;

                AudioDevices = await GetAudioOutputDevices();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }

    private async Task SetParametersAsync()
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
    }

    private async Task SetComfortableToneAsync()
    {
        Type = Types.Sine;
        Frequency = 528;
        await SetParametersAsync();
    }

    private async Task<List<AudioDevice>?> GetAudioOutputDevices()
    {
        if (module is null)
        {
            return null;
        }

        var devices = (await module.GetAudioOutputDevices()).ToList();
        if (devices is null)
        {
            return null;
        }

        devices = devices
            .OrderBy(device => device.Label)
            .ThenBy(device => device.GroupId)
            .Except(devices.Where(device =>
                device is { DeviceId: null } ||
                device.DeviceId.Equals("communications", StringComparison.OrdinalIgnoreCase)))
            .ToList();

        var defaultGroupId = devices
            .Where(device =>
                device is { DeviceId: not null } &&
                device.DeviceId.Equals("default", StringComparison.OrdinalIgnoreCase))
            .Select(device => device.GroupId)
            .FirstOrDefault();

        devices = devices
            .Except(devices.Where(device =>
                device is { DeviceId: null } ||
                device.DeviceId.Equals("default", StringComparison.OrdinalIgnoreCase)))
            .ToList();

        AudioDevice? defaultDevice = null;

        foreach (var device in devices)
        {
            if (device is { GroupId: not null } && device.GroupId.Equals(defaultGroupId, StringComparison.OrdinalIgnoreCase))
            {
                defaultDevice = device;
                break;
            }
        }

        if (defaultDevice is not null)
        {
            var index = devices.IndexOf(defaultDevice);
            devices[index] = devices[index] with { IsDefault = true };
        }

        return devices;
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
            SetAudioDevice(SelectedDeviceId);
        }
    }
}

#pragma warning restore BL0007
#pragma warning restore CS4014
#pragma warning restore IDE0058
