using Microsoft.JSInterop;

namespace SoundTest;

public class JsInterop : IAsyncDisposable
{
    private readonly Lazy<Task<IJSObjectReference>> moduleTask;

    public JsInterop(IJSRuntime jsRuntime) =>
        moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>("import", $"./JsInterop.js").AsTask());

    public async ValueTask DisposeAsync()
    {
        if (moduleTask.IsValueCreated)
        {
            IJSObjectReference module = await moduleTask.Value;
            await module.DisposeAsync();
        }
    }

    private async Task InvokeVoidAsync(string method, params object[] args) =>
        await (await moduleTask.Value).InvokeVoidAsync(method, args);

    public async Task SetParameters(string type = "sine", int frequency = 440) =>
        await InvokeVoidAsync("setParameters", type, frequency);

    public async Task StartPlaying() =>
        await InvokeVoidAsync("startPlaying");

    public async Task StopPlaying() =>
        await InvokeVoidAsync("stopPlaying");

    public async Task CopyTextToClipboard(string textToCopy) =>
        await InvokeVoidAsync("copyTextToClipboard", textToCopy);
}
