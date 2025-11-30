using AADizErp.ViewModels.RequestVM;

namespace AADizErp.Pages.RequestPages;

public partial class AttendanceRequestPage : ContentPage
{
	public AttendanceRequestPage(RemoteAttendancePageViewModel viewmodel)
	{
		InitializeComponent();
		BindingContext = viewmodel;
	}

    private async void SubmitNewAttendanceRequestButton_Tap(object sender, DevExpress.Maui.Core.DXTapEventArgs e)
    {  
		await Shell.Current.GoToAsync($"{nameof(SaveAttendancePage)}");
    }
}