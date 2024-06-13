using AADizErp.ViewModels.SettingsVM;

namespace AADizErp.Pages.SettingsPages;

public partial class PasswordChangePage : ContentPage
{
	public PasswordChangePage(PasswordChangePageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}