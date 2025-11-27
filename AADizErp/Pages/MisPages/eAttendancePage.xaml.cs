using AADizErp.ViewModels.MisPageVM;

namespace AADizErp.Pages.MisPages;

public partial class eAttendancePage : ContentPage
{
	public eAttendancePage(Mis_eAttendancePageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext= viewModel;
    }
}