
namespace AADizErp.Pages.SettingsPages;

public partial class SettingsLandingPage : ContentPage
{
	public SettingsLandingPage()
	{
		InitializeComponent();
	}

    private async void PasswordChangedPage_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"{nameof(PasswordChangePage)}");
    }

    private async void DeleteAccountPage_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"{nameof(DeleteAccountPage)}");
    }
}