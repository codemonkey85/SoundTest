namespace SoundTest.Pages;

public partial class Index
{
    [Parameter, SupplyParameterFromQuery]
    public int Type { get; set; }

    [Parameter, SupplyParameterFromQuery]
    public int Frequency { get; set; }
}
