using AADizErp.ViewModels.HolidayVm;

namespace AADizErp.Pages.HolidayPages;

public partial class HolidayViewPage : ContentPage
{
	public HolidayViewPage(HolidayViewPageViewModel viewmodel)
	{
		InitializeComponent();
		BindingContext = viewmodel;
	}
}