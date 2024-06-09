using AADizErp.ViewModels.RequestVM;

namespace AADizErp.Pages.RequestPages;

public partial class LeaveRequestFormPopup : ContentPage
{
    public LeaveRequestFormPopup(LeaveRequestPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}