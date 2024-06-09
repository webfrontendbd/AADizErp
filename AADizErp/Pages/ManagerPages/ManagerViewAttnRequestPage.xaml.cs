using AADizErp.ViewModels.ManagerPagesVM;

namespace AADizErp.Pages.ManagerPages;

public partial class ManagerViewAttnRequestPage : ContentPage
{
	public ManagerViewAttnRequestPage(ManagerAttendanceListViewModel viewModel)
    {
		InitializeComponent();
		BindingContext = viewModel;
	}
}