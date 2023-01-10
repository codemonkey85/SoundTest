#pragma warning disable BL0007
#pragma warning disable CS4014
#pragma warning disable IDE0058

namespace SoundTest.Components;

public partial class SoundComponent
{
    private bool isPlaying = false;
    private string? soundLink;

    private Types type;
    private int frequency;

    private bool isJsInitialized = false;

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
                isJsInitialized = true;
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
}

#pragma warning restore BL0007
#pragma warning restore CS4014
#pragma warning restore IDE0058
