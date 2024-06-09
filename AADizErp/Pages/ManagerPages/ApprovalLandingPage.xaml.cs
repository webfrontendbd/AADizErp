namespace AADizErp.Pages.ManagerPages;

public partial class ApprovalLandingPage : ContentPage
{
	public ApprovalLandingPage()
	{
		InitializeComponent();
	}
    private async void AttendanceForApprovalMenu_Tapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"{nameof(ManagerViewAttnRequestPage)}");
    }

    private async void LeaveApprovalMenu_Tapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"{nameof(ManagerLeaveViewPage)}");
    }
}