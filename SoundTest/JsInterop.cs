using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace SoundTest
{
    public class JsInterop : IAsyncDisposable
    {
        private readonly Lazy<Task<IJSObjectReference>> moduleTask;

        #region Constructor / Destructor

        public JsInterop(IJSRuntime jsRuntime)
        {
            moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
               "import", $"./JsInterop.js").AsTask());
        }

        public async ValueTask DisposeAsync()
        {
            if (moduleTask.IsValueCreated)
            {
                IJSObjectReference module = await GetModule();
                await module.DisposeAsync();
            }
        }

        #endregion

        #region Private methods

        private async Task<IJSObjectReference> GetModule() => await moduleTask.Value;

        private async Task<T> InvokeAsync<T>(string method, params object[] args)
        {
            IJSObjectReference module = await GetModule();
            return await module.InvokeAsync<T>(method, args);
        }

        private async Task InvokeVoidAsync(string method, params object[] args)
        {
            IJSObjectReference module = await GetModule();
            await module.InvokeVoidAsync(method, args);
        }

        #endregion

        public async Task InitializeSoundGenerator() =>
            await InvokeVoidAsync("initializeSoundGenerator");

        public async Task SetParameters(string type = "sine", int frequency = 440) =>
            await InvokeVoidAsync("setParameters", type, frequency);

        public async Task StartPlaying() =>
            await InvokeVoidAsync("startPlaying");

        public async Task StopPlaying() =>
            await InvokeVoidAsync("stopPlaying");
    }
}
