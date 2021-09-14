using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using static SoundTest.Constants;

namespace SoundTest.Components
{
    public partial class SoundTestComponent
    {
        private bool isPlaying = false;

        private Types type = DefaultType;
        private int frequency = DefaultFrequency;

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
            }
        }

        private string SoundLink => $"{Navigation.BaseUri}?{nameof(type)}={type}&{nameof(frequency)}={frequency}";

        private async Task SetParameters() =>
            await JsInterop.SetParameters(Type.ToString().ToLower(), Frequency);

        private async Task StartPlaying()
        {
            await JsInterop.StartPlaying();
            isPlaying = true;
        }

        private async Task StopPlaying()
        {
            await JsInterop.StopPlaying();
            isPlaying = false;
        }

        private async Task CopySoundLink() =>
            await JsInterop.CopyTextToClipboard(SoundLink);
    }
}
