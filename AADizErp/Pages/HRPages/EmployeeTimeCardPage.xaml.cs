using AADizErp.ViewModels.HrVM;

namespace AADizErp.Pages.HRPages;

public partial class EmployeeTimeCardPage : ContentPage
{
	public EmployeeTimeCardPage(EmployeeTimeCardPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext=viewModel;
	}
}