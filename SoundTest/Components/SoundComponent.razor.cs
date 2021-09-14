namespace SoundTest.Components
{
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

        private void UpdateUri()
        {
            string? uri = Navigation.GetUriWithQueryParameters(new Dictionary<string, object>()
            {
                [nameof(type)] = (int)type,
                [nameof(frequency)] = frequency,
            });
            soundLink = uri.ToString();
        }

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
            await JsInterop.CopyTextToClipboard(soundLink);
    }
}
