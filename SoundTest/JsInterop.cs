namespace SoundTest;

// ReSharper disable once ClassNeverInstantiated.Global
public partial class JsInterop
{
    private const string JsFileName = "soundtest.js";

    [JSImport(nameof(SetParameters), JsFileName)]
    public static partial void SetParameters(string type, int frequency);

    [JSImport(nameof(StartPlaying), JsFileName)]
    public static partial void StartPlaying();

    [JSImport(nameof(StopPlaying), JsFileName)]
    public static partial void StopPlaying();

    [JSImport(nameof(CopyTextToClipboard), JsFileName)]
    public static partial void CopyTextToClipboard(string text);
}

// ReSharper disable once InconsistentNaming
public static class IJSObjectReferenceExtensions
{
    extension(IJSObjectReference jsModule)
    {
        public async Task<IEnumerable<AudioDevice>> GetAudioOutputDevices()
        {
            try
            {
                return await jsModule.InvokeAsync<IEnumerable<AudioDevice>>(nameof(GetAudioOutputDevices));
            }
            catch (Microsoft.JSInterop.JSException ex)
            {
                throw new InvalidOperationException("Unable to enumerate audio output devices.", ex);
            }
        }

        public async Task SetAudioDevice(string deviceId)
        {
            try
            {
                await jsModule.InvokeVoidAsync(nameof(SetAudioDevice), deviceId);
            }
            catch (Microsoft.JSInterop.JSException ex)
            {
                throw new InvalidOperationException("Unable to select the audio output device.", ex);
            }
        }
    }
}

public record AudioDevice(string? DeviceId, string? Label, string? GroupId, bool IsDefault);
