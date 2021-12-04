namespace SoundTest.Components;

public partial class SoundComponent
{
    private bool isPlaying = false;
    private string? soundLink;

    private Types type;
    private int frequency;

    [Parameter]
    public Types Type
    {
        get => type;
        set
        {
            type = value;
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            SetParameters();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
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
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            SetParameters();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            UpdateUri();
        }
    }

    private JsInterop? jsInterop;

    private JsInterop JsInterop
    {
        get
        {
            if (jsInterop == null)
            {
                jsInterop = new JsInterop(JSRuntime, $"./{nameof(Components)}/{nameof(SoundComponent)}.razor.js");
            }
            return jsInterop;
        }
    }

    private void UpdateUri()
    {
        string? uri = Navigation.GetUriWithQueryParameters(new Dictionary<string, object?>()
        {
            [nameof(type)] = (int)type,
            [nameof(frequency)] = frequency,
        });
        soundLink = uri.ToString();
    }

    private async Task SetParameters() =>
        await JsInterop.InvokeVoidAsync("setParameters", Type.ToString().ToLower(), Frequency);

    private async Task StartPlaying()
    {
        await JsInterop.InvokeVoidAsync("startPlaying");
        isPlaying = true;
    }

    private async Task StopPlaying()
    {
        await JsInterop.InvokeVoidAsync("stopPlaying");
        isPlaying = false;
    }

    private async Task CopySoundLink()
    {
        if (soundLink == null)
        {
            return;
        }
        await JsInterop.InvokeVoidAsync("copyTextToClipboard", soundLink);
    }
    
    private async Task SetComfortableTone()
    {
        Type = Types.Sine;
        Frequency = 528;
        await SetParameters();
    }
}
