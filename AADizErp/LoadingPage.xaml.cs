using AADizErp.ViewModels;

namespace AADizErp;

public partial class LoadingPage : ContentPage
{
	public LoadingPage(LoadingViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}