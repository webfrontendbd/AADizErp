using AADizErp.ViewModels.McPageVM;

namespace AADizErp.Pages.NptPages;

public partial class MachineInfoPage : ContentPage
{
	public MachineInfoPage(MachineInfoPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
    }
}