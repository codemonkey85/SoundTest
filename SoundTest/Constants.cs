namespace SoundTest;

public static class Constants
{
    public const int MinFrequency = 1;

    public const int MaxFrequency = 1500; // 20154;

    public const int ComfortableFrequency = 528;

    public const int NeuroFrequency = 852;

    public enum Types
    {
        Sine,
        Square,
        Triangle,
        Sawtooth,
    }
}
