using AADizErp.ViewModels.SalaryPagesVM;

namespace AADizErp.Pages.SalaryPages;

public partial class PaySlipListViewPage : ContentPage
{
	public PaySlipListViewPage(PaySlipListViewPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}