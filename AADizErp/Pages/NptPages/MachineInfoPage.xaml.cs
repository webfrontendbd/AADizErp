using AADizErp.ViewModels.McPageVM;
using DevExpress.Maui.Core;
using static System.Net.Mime.MediaTypeNames;

namespace AADizErp.Pages.NptPages;

public partial class MachineInfoPage : ContentPage
{
	public MachineInfoPage(MachineInfoPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
    }

    private void MachineUpdateButton_Tapped(object sender, DevExpress.Maui.Core.DXTapEventArgs e)
    {
        var button = sender as DXButton;
        // Get viewmodel
        var vm = button.BindingContext as MachineInfoPageViewModel;
        string mcid = vm?.Mcid;
        Shell.Current.GoToAsync(
            $"{nameof(McStatusUpdatePage)}?Mcid={Uri.EscapeDataString(mcid)}"
        );
    }

}