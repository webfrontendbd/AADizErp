using AADizErp.ViewModels.ManagerPagesVM;

namespace AADizErp.Pages.ManagerPages;

public partial class ApprovalLandingPage : ContentPage
{
	public ApprovalLandingPage(ApprovalLandingPageViewModel viewmodel)
	{
		InitializeComponent();
        BindingContext=viewmodel;
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