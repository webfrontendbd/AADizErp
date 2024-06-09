using AADizErp.ViewModels.HrVM;

namespace AADizErp.Pages.HRPages;

public partial class EmployeeFileQueryPage : ContentPage
{
	public EmployeeFileQueryPage(EmployeeFileQueryPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext=viewModel;
	}
}