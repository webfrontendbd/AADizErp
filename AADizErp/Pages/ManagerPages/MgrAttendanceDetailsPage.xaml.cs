using AADizErp.ViewModels.ManagerPagesVM;
namespace AADizErp.Pages.ManagerPages;

public partial class MgrAttendanceDetailsPage : ContentPage
{ 
    public MgrAttendanceDetailsPage(MgrAttendanceDetailsPageViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
	
}