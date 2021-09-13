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

        public async Task GenerateSound(string type = "sine", int frequency = 440, int stopTime = 1) =>
            await InvokeVoidAsync("generateSound", type, frequency, stopTime);
    }
}
