using AADizErp.ViewModels.ManagerPagesVM;

namespace AADizErp.Pages.ManagerPages;

public partial class ManagerLeaveViewPage : ContentPage
{
	public ManagerLeaveViewPage(ManagerLeaveListViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

    //private void LeaveAction_Clicked(object sender, EventArgs e)
    //{
    //    actionsPopup.PlacementTarget = (View)sender;
    //    actionsPopup.IsOpen = !actionsPopup.IsOpen;
    //}

    //private void LeavePopupHide_Clicked(object sender, EventArgs e)
    //{
    //    actionsPopup.IsOpen = false;
    //}
}