using AADizErp.ViewModels.HrVM;

namespace AADizErp.Pages.HRPages;

public partial class DailyAbsentDetailPage : ContentPage
{
	public DailyAbsentDetailPage(DailyAbsentDetailPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}