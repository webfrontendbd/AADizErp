using AADizErp.ViewModels.RequestVM;

namespace AADizErp.Pages.RequestPages;

public partial class AttendanceRequestPage : ContentPage
{
	public AttendanceRequestPage(RemoteAttendancePageViewModel viewmodel)
	{
		InitializeComponent();
		BindingContext = viewmodel;
	}

}