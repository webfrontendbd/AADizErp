using AADizErp.ViewModels.HrVM;

namespace AADizErp.Pages.HRPages;

public partial class AddUnconfirmEmployeePage : ContentPage
{
	public AddUnconfirmEmployeePage(AddEmployeeViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
    }
}