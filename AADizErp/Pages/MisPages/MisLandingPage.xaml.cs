namespace AADizErp.Pages.MisPages;

public partial class MisLandingPage : ContentPage
{
	public MisLandingPage()
	{
		InitializeComponent();
	}
    private async void OvertimeMenu_Tapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"{nameof(OverTimePage)}");
    }

    private async void eAttendanceRequestMenu_Tapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"{nameof(eAttendancePage)}");
    }

    private async void MachineStatusMenu_Tapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"{nameof(MachinesPage)}");
    }
    private async void ProductionStatusMenu_Tapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"{nameof(ProductionPage)}");
    }
}