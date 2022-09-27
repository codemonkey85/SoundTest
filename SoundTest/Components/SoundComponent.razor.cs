using System.Diagnostics;

namespace SoundTest.Components;

public partial class SoundComponent
{
    private bool isPlaying = false;
    private string? soundLink;

    private Types type;
    private int frequency;

    private bool IsJsInitialized = false;

    [Parameter]
    public Types Type
    {
        get => type;
        set
        {
            type = value;
            SetParametersAsync();
            UpdateUri();
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
            SetParametersAsync();
            UpdateUri();
        }
    }

    private void UpdateUri()
    {
        var uri = Navigation.GetUriWithQueryParameters(new Dictionary<string, object?>()
        {
            [nameof(type)] = (int)type,
            [nameof(frequency)] = frequency,
        });
        soundLink = uri.ToString();
    }

    protected override async Task OnInitializedAsync() => await InitializeJs();

    private async Task InitializeJs()
    {
        if (OperatingSystem.IsBrowser() && !IsJsInitialized)
        {
            try
            {
                var result = await JSHost.ImportAsync("soundtest.js", $"../{nameof(Components)}/{nameof(SoundComponent)}.razor.js");
                if (result is not null)
                {
                    IsJsInitialized = true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }

    private async Task SetParametersAsync()
    {
        if (!IsJsInitialized)
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
