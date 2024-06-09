using AADizErp.ViewModels.HrVM;

namespace AADizErp.Pages.HRPages;

public partial class ProfileViewPage : ContentPage
{
	public ProfileViewPage(ProfilePageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}