namespace SoundTest;

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

public static class IJSObjectReferenceExtensions
{
    public static async Task<IEnumerable<AudioDevice>> GetAudioOutputDevices(this IJSObjectReference jsModule) =>
        await jsModule.InvokeAsync<IEnumerable<AudioDevice>>(nameof(GetAudioOutputDevices));

    public static async Task SetAudioDevice(this IJSObjectReference jsModule, string deviceId) =>
        await jsModule.InvokeVoidAsync(nameof(SetAudioDevice), deviceId);
}

public record AudioDevice(string? DeviceId, string? Label, string? GroupId, bool IsDefault);
