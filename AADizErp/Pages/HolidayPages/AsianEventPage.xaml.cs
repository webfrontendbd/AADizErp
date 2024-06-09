using AADizErp.ViewModels.HolidayVm;

namespace AADizErp.Pages.HolidayPages;

public partial class AsianEventPage : ContentPage
{
	public AsianEventPage(AsianEventViewModel viewmodel)
	{
		InitializeComponent();
		BindingContext = viewmodel;
	}
}