namespace SoundTest.Pages;

public partial class Home
{
    [Parameter, SupplyParameterFromQuery]
    public int Type { get; set; }

    [Parameter, SupplyParameterFromQuery]
    public int Frequency { get; set; }
}
