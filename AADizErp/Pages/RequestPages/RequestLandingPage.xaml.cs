namespace AADizErp.Pages.RequestPages;

public partial class RequestLandingPage : ContentPage
{
	public RequestLandingPage()
	{
		InitializeComponent();
	}
    private async void AttendanceMenu_Tapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"{nameof(AttendanceRequestPage)}");
    }

    private async void LeaveRequestMenu_Tapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"{nameof(LeaveRequestPage)}");
    }
}