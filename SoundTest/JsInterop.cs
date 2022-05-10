namespace SoundTest;

public class JsInterop : IAsyncDisposable
{
    private readonly Lazy<Task<IJSObjectReference>> moduleTask;

    public JsInterop(IJSRuntime jsRuntime, string jsFilePath) =>
        moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>("import", jsFilePath).AsTask());

    public async ValueTask DisposeAsync()
    {
        if (moduleTask.IsValueCreated)
        {
            var module = await moduleTask.Value;
            await module.DisposeAsync();
        }
    }

    public async Task InvokeAsync<ReturnType>(string method, params object[] args) =>
        await (await moduleTask.Value).InvokeAsync<ReturnType>(method, args);

    public async Task InvokeVoidAsync(string method, params object[] args) =>
        await (await moduleTask.Value).InvokeVoidAsync(method, args);
}
