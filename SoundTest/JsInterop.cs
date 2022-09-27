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
