#pragma warning disable CS4014

namespace SoundTest.Components;

public partial class SoundComponent
{
    private bool _isPlaying = false;
    private string? _soundLink;

    private Types _type;
    private int _frequency;

    private bool _isJsInitialized = false;

    [Parameter]
    public Types Type
    {
        get => _type;
        set
        {
            _type = value;
            SetParametersAsync();
            UpdateUri();
        }
    }

    [Parameter]
    public int Frequency
    {
        get => _frequency;
        set
        {
            _frequency = value switch
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
            [nameof(_type)] = (int)_type,
            [nameof(_frequency)] = _frequency,
        });
        _soundLink = uri;
    }

    protected override async Task OnInitializedAsync() => await InitializeJs();

    private async Task InitializeJs()
    {
        if (OperatingSystem.IsBrowser() && !_isJsInitialized)
        {
            try
            {
                await JSHost.ImportAsync("soundtest.js",
                    $"../{nameof(Components)}/{nameof(SoundComponent)}.razor.js");
                _isJsInitialized = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }

    private async Task SetParametersAsync()
    {
        if (!_isJsInitialized)
        {
            await InitializeJs();
        }

        JsInterop.SetParameters(Type.ToString().ToLower(), Frequency);
    }

    private void StartPlaying()
    {
        JsInterop.StartPlaying();
        _isPlaying = true;
    }

    private void StopPlaying()
    {
        JsInterop.StopPlaying();
        _isPlaying = false;
    }

    private void CopySoundLink()
    {
        if (_soundLink == null)
        {
            return;
        }

        JsInterop.CopyTextToClipboard(_soundLink);
    }

    private async Task SetComfortableToneAsync()
    {
        Type = Types.Sine;
        Frequency = 528;
        await SetParametersAsync();
    }
}
