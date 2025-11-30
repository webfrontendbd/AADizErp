using AADizErp.ViewModels.RequestVM;
using DevExpress.Maui.Core;

namespace AADizErp.Pages.RequestPages;

public partial class SaveAttendancePage : ContentPage
{
    public SaveAttendancePage(RemoteAttendancePageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
    }
    
}