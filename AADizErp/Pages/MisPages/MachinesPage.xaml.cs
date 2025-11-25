using AADizErp.ViewModels.MisPageVM;

namespace AADizErp.Pages.MisPages;

public partial class MachinesPage : ContentPage
{
	public MachinesPage(GroupMachineStatusPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
    }
}