using AADizErp.ViewModels.SettingsVM;

namespace AADizErp.Pages.SettingsPages;

public partial class DeleteAccountPage : ContentPage
{
	public DeleteAccountPage(DeleteAccountPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}