using Microsoft.JSInterop;

namespace SoundTest;

public partial class JsInterop
{
    [JSImport(nameof(SetParameters), "soundtest.js")]
    public static partial void SetParameters(string type, int frequency);

    [JSImport(nameof(StartPlaying), "soundtest.js")]
    public static partial void StartPlaying();

    [JSImport(nameof(StopPlaying), "soundtest.js")]
    public static partial void StopPlaying();

    [JSImport(nameof(CopyTextToClipboard), "soundtest.js")]
    public static partial void CopyTextToClipboard(string text);
}

public static class IJSObjectReferenceExtensions
{
    public static async Task<IEnumerable<AudioDevice>> GetAudioOutputDevices(this IJSObjectReference jsModule)
    {
        return await jsModule.InvokeAsync<IEnumerable<AudioDevice>>(nameof(GetAudioOutputDevices));
    }

    public static async Task SetAudioDevice(this IJSObjectReference jsModule, string deviceId)
    {
        await jsModule.InvokeVoidAsync(nameof(SetAudioDevice), deviceId);
    }
}

public record AudioDevice(string? DeviceId, string? Label, string? GroupId, bool IsDefault);
