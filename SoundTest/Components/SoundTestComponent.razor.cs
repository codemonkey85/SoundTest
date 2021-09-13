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

        private Types Type
        {
            get => type;
            set
            {
                type = value;
                SetParameters();
                //if (isPlaying)
                //{
                //    StopPlaying();
                //    StartPlaying();
                //}
            }
        }

        private int Frequency
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
                SetParameters();
                //if (isPlaying)
                //{
                //    StopPlaying();
                //    StartPlaying();
                //}
            }
        }

        protected override async Task OnInitializedAsync() =>
            await JsInterop.InitializeSoundGenerator();

        //private async Task OnChangeType(ChangeEventArgs e)
        //{
        //    type = int.TryParse(e.Value?.ToString(), out int newTypeInt)
        //        ? newTypeInt switch
        //        {
        //            (int)Types.Sine or (int)Types.Square or (int)Types.Triangle or (int)Types.Sawtooth => (Types)newTypeInt,
        //            _ => DefaultType,
        //        }
        //        : DefaultType;
        //    await SetParameters();
        //    if (isPlaying)
        //    {
        //        await StopPlaying();
        //        await StartPlaying();
        //    }
        //}

        //private async Task OnChangeFrequency(ChangeEventArgs e)
        //{
        //    frequency = int.TryParse(e.Value?.ToString(), out int newFrequency)
        //        ? newFrequency
        //        : DefaultFrequency;
        //    await SetParameters();
        //    if (isPlaying)
        //    {
        //        await StopPlaying();
        //        await StartPlaying();
        //    }
        //}

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
    }
}
