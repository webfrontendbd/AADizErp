namespace AADizErp.Pages.NptPages;

public partial class NptLandingPage : ContentPage
{
	public NptLandingPage()
	{
		InitializeComponent();
	}

    private async void McScanPageButton_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"{nameof(McBarcodeScanPage)}");
    }
}