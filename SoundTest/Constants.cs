namespace SoundTest
{
    public static class Constants
    {
        public const int MinFrequency = 1;

        public const int MaxFrequency = 1500; // 20154;

        public const int DefaultFrequency = 440;

        public const Types DefaultType = Types.Sine;

        public enum Types
        {
            Sine,
            Square,
            Triangle,
            Sawtooth,
        }
    }
}
